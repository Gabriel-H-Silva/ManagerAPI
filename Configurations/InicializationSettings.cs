using MySqlConnector;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Security.Cryptography;
using ManagerApi.DM;

namespace ManagerApi.Configurations
{

    public class InicializationSettings
    {
        public string? GlobalConnetionString { get; set; }
        public bool SoftwareStatus { get; set; } = false;
    }

    public class Settings
    {

        //-----------------------------------------------\\
        //   Configurações de Inicialização do Software  \\
        //-----------------------------------------------\\

        // arquivos gerados na Inicialização
        const string verPath = "VerSa.ini";
        const string configPath = "Config.ini";

        // string que recebe o IP do arquivo Config.ini
        string IPConfig = "";

        // conexao do banco local
        string pConnectionString = "";

        // codigo hash das configurações do computador
        long hashComputer;

        // Metodo principal que chama todos os eventos
        public InicializationSettings Initialization()
        {
            InicializationSettings inicialization = new InicializationSettings();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();

            // Verifica se o arquivo Config.ini existe. Se não existir ele vai constar como um novo cliente e chamar o metodo para criar o arquivo Config.ini
            if (!File.Exists(configPath)) CreateConfigFile();

            // metodo para coletar o IP do cliente
            IPConfig = GetIP();

            // metodo para se testar a comunicação com o banco de dados
            ConnectDatabase();

            // Verifica se o arquivo VerSa.ini existe. Se não existir ele vai criptografar os dados e cadastrar o cliente novo
            if (!File.Exists(verPath)) NewClient(); 

            // metodo para validar o software
            if (ValidationSoftware())
            {
                // metodo para a IO aprovar o gerenciamento
                if (ValidationIOElectronic())
                {
                    // metodo para contar as maquinas
                    MachinesDM pMachines = CountTotalMachines();

                    Console.Clear();
                    Console.WriteLine("VisionManager - " + DateTime.Now + " IO Eletronica.\n");
                    Console.WriteLine("Preço: 0");
                    Console.WriteLine("\nMaquinas Cadastradas: " + pMachines.Total_Machines + " | Maquinas Ativas: " + pMachines.Active_Total_Machines + " | Maquinas Defeito: " + pMachines.Error_Total_Machines);
                    inicialization.SoftwareStatus = true;
                    inicialization.GlobalConnetionString = pConnectionString;

                    return inicialization;
                    
                }
            }
            return inicialization;
        }

        // Metodo para criar o arquivo Config.ini
        private void CreateConfigFile()
        {
            try
            {
                File.Create(configPath).Close();

                using (StreamWriter w = File.AppendText(configPath))
                {
                    w.WriteLine("IPVisonDB=" + IPConfig);
                    w.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Metodo para a IO verificar o versa.ini
        private bool ValidationSoftware()
        {
            string combinedString = GetPCInfo();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\nVALIDAÇAO DE SOFTWARE: ");
            string linha;
            long compiledCode = HashCombinedString(combinedString);
            bool retorno = false;
            try
            {
                using (StreamReader sr = new StreamReader(verPath))
                {

                    linha = sr.ReadLine();
                }

                if (!string.IsNullOrEmpty(linha))
                {
                    if (linha == "IO" + compiledCode.ToString())
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("OK");
                        hashComputer = compiledCode;
                        Console.ForegroundColor = ConsoleColor.White;
                        retorno = true;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Beep();
                        Console.Write("FALHA");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Falha na leitura do Arquivo");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Erro ao ler o arquivo: {ex.Message}");
                Console.ReadKey();
                Environment.Exit(0);
            }
            return retorno;
        }

        // Metodo de criptografar as informaçoes do computador
        private long HashCombinedString(string combinedString)
        {
            ulong hashValue;
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(combinedString));
                hashValue = BitConverter.ToUInt64(bytes, 0);
            }
            long parseLong = (long)hashValue;
            return parseLong;
        }


        // -------------------------------------------- \\
        //     Metodos que Utiliza p banco de dados     \\
        // -------------------------------------------- \\

        // Metodo para testar a comunicação com o banco de dados e gerar a conexao
        private void ConnectDatabase()
        {
            Console.Write("\nCOMUNICAÇÂO BANCO: ");
            string lConnectionString = "Server=" + IPConfig + ";Database=visionbd;Uid=root;Pwd=root;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(lConnectionString))
                {
                    connection.Open();

                    string query = "SELECT 1";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        int result = Convert.ToInt32(command.ExecuteScalar());
                        if (result == 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("OK");
                            Console.ForegroundColor = ConsoleColor.White;

                            pConnectionString = lConnectionString;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("ERROR");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.ReadKey();
                            Environment.Exit(0);
                        }
                    }

                    connection.Close();
                }

            }
            catch (MySqlException ex)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("Erro ao conectar com o banco de dados " + IPConfig + "\nse o banco estiver em outro IP aplique a configuração no arquivo corretamente");
                Console.ReadKey();
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine($"Erro inesperado: {ex.Message}");
                Console.ReadKey();
                Environment.Exit(0);
            }

        }

        // Metodo para a contar as maquinas
        private MachinesDM CountTotalMachines()
        {

            MachinesDM machines = new MachinesDM();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(pConnectionString))
                {
                    connection.Open();

                    string query = $"SELECT COUNT(id) FROM tb_machines";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                machines.Total_Machines = reader.GetInt32(0);
                            }
                        }
                    }

                    query = $"SELECT COUNT(id) FROM tb_machines WHERE status = 'A'";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                machines.Active_Total_Machines = reader.GetInt32(0);
                            }
                        }
                    }

                    query = $"SELECT COUNT(id) FROM tb_machines WHERE status = 'I'";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                machines.Error_Total_Machines = reader.GetInt32(0);
                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return machines;
        }

        // Metodo para salvar a versa no banco de dados - para a io validar
        private bool SaveVersionDataBase(long compiledCode, string clientName)
        {
            bool retorno = false;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(pConnectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM tb_config";
                    //using (MySqlCommand command = new MySqlCommand(query, connection))
                    //{
                    //    command.ExecuteNonQuery();
                    //}

                    query = "INSERT INTO tb_config (versao, client, client_create) VALUES ('IO" + compiledCode + "','" + clientName + "', NOW())";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                        retorno = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao inserir dados: {ex.Message}");
                Console.ReadKey();
                Environment.Exit(0);
            }
            return retorno;
        }

        // Metodo para cadastrar um novo Cliente
        private void NewClient()
        {

            Console.WriteLine("\nCadastro de novo cliente");
            Console.Write("Loja: ");
            string clientNome;

            while (true)
            {
                clientNome = Console.ReadLine();
                if (!string.IsNullOrEmpty(clientNome)) break;
            }


            string combinedString = GetPCInfo();

            try
            {
                long compiledCode = HashCombinedString(combinedString);

                if (SaveVersionDataBase(compiledCode, clientNome))
                {
                    File.Create(verPath).Close();

                    using (StreamWriter w = File.AppendText(verPath))
                    {
                        w.WriteLine("IO" + compiledCode);
                        w.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Metodo para a IO aprovar o gerenciamento
        private bool ValidationIOElectronic()
        {
            Console.Write("\nVALIDAÇAO DO SISTEMA: ");
            string code = "";
            string status = "";
            DateTime? expiration = null;

            bool retorno = false;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(pConnectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM tb_config WHERE versao = 'IO" + hashComputer +"'";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                code = reader["versao"].ToString();
                                status = reader["status"].ToString();
                                if(status != "W")
                                {
                                   expiration = (DateTime)reader["contract_expiration"];
                                }
                               
                            }
                        }
                    }

                    if (expiration != null && expiration >= DateTime.Now)
                    {
                        query = "UPDATE tb_config SET status = 'E' WHERE versao = 'IO" + code +"'";
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            using (MySqlDataReader reader = command.ExecuteReader()) ;
                        }
                    }

                    connection.Close();
                }
                

                if (code == "IO"+hashComputer && status == "A" || status == "E")
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("OK");
                    Console.ForegroundColor = ConsoleColor.White;
                    retorno = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Software sem permissão \nReinicie o Gerenciamento ou aguarde a IO Eletronica aprovar o software!");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadKey();
                Environment.Exit(0);
            }
            return retorno;
        }

        // -------------------------------------------- \\
        // Metodos de Coletar informaçoes do computador \\
        // -------------------------------------------- \\

        // Metodo para pegar o IP
        private string GetIP()
        {
            string chave = "IPVisonDB=";
            string tIp = "";

            try
            {
                using (StreamReader sr = new StreamReader(configPath))
                {
                    string linha;
                    while ((linha = sr.ReadLine()) != null)
                    {
                        if (linha.Trim().ToLower().Contains(chave.ToLower()))
                        {
                            // Remove a chave e espaços em branco, se houver, e obtém o valor
                            tIp = linha.Replace(chave, "").Trim();
                            break; // Encontra a linha desejada e sai do loop
                        }
                    }
                }

                if (string.IsNullOrEmpty(tIp))
                {
                    tIp = GetMyIP();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao ler o arquivo: {ex.Message}");
                Console.ReadKey();
            }

            return tIp;
        }
        private string GetPCInfo()
        {
            string combinedString;
            // Coleta informações do processador
            string cpuInfo = GetProcessorInfo();

            // Coleta informações da memória
            string memoryInfo = GetMemoryInfo();

            // Coleta informações do disco
            string diskInfo = GetDiskInfo();

            combinedString = memoryInfo + cpuInfo + diskInfo;

            return combinedString;
        }
        private string GetMyIP()
        {
            string output = "";

            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            return output = ip.Address.ToString();
                        }
                    }
                }
            }
            return output;
        }
        private string GetProcessorInfo()
        {
            string cpuId = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_Processor");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                // Tente obter um identificador único, mas isso pode variar
                cpuId = queryObj["ProcessorId"].ToString();
                break;
            }
            return cpuId;
        }
        private string GetMemoryInfo()
        {
            string totalMemory = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                totalMemory = queryObj["TotalVisibleMemorySize"].ToString();
                break;
            }
            return totalMemory;
        }
        private string GetDiskInfo()
        {
            string diskId = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_DiskDrive");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                // Tente obter um identificador único, mas isso pode variar
                diskId = queryObj["SerialNumber"].ToString();
                break;
            }
            return diskId;
        }
    }
}
