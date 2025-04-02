<h1>BankAccountAPI</h1>

<p>BankAccountAPI é uma API simples para gerenciar contas bancárias, permitindo o registro de novas contas, consulta de saldo, extrato de transações e transferências entre contas.</p>

<h2>Requisitos</h2>
  <ul>
    <li>.NET 8.0</li>
    <li>SQL Server</li>
    <li>Visual Studio 2022 (ou qualquer outro IDE compatível com .NET)</li>
  </ul>
  
<h2>Configuração do Projeto</h2>
  <ol>
    <li><strong>Clone o repositório:</strong>
        <ul>
            <li>git clone <href>https://github.com/seu-usuario/BankAccountAPI.git</href></li>
            <li>cd BankAccountAPI</li>
        </ul>
    </li>
    <li><strong>Configurar a string de conexão:</strong> Abra o arquivo appsettings.json e configure a string de conexão para o seu banco de dados SQL Server:
        <pre><code>
                     {
     "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
       }
     },
     "ConnectionStrings": {
       "DefaultConnection": "Server=SEU_SERVIDOR;Database=BankDB;User Id=SEU_USUARIO;Password=SUA_SENHA;Trusted_Connection=True;TrustServerCertificate=True;"
     },
     "AllowedHosts": "*"
   }</code></pre>
    </li>
    <li><strong>Instalar as dependências:</strong> No Visual Studio, abra o Console do Gerenciador de Pacotes NuGet e execute o seguinte comando para instalar as dependências:
        <pre><code>
             dotnet restore
        </code></pre>
    </li>
    <li>
      <strong>Adicionar e aplicar migrações:</strong> No Console do Gerenciador de Pacotes NuGet, execute os seguintes comandos para adicionar e aplicar migrações ao banco de dados:
            <ul>    
                <li>Add-Migration InitialCreate</li>
                 <li>Update-Database</li>
            </ul>
    </li>
    
  <h2>Endpoints da API</h2>
  <h3>Registrar uma nova conta</h3>
      <ul><strong>
        <li>URL: /api/accounts/register</li>
        <li>Método: POST</li>
        <li>Corpo da Requisição:</li></strong>
            <pre><code>
                {
                  "name": "string",
                  "email": "string",
                  "password": "string"
                }
            </code></pre>
      </ul>

  <h3>Consultar saldo</h3>
      <ul>
        <strong><li>URL: /api/accounts/balance</li>
        <li>Método: GET</li>
        <li>Parâmetros de Consulta:</strong>
          <ul><li>accountNumber: Número da conta</li></ul>
        </li>
      </ul>

<h3>Consultar extrato</h3>
      <ul>
        <strong><li>URL: /api/accounts/statement</li>
        <li>Método: GET</li>
        <li>Parâmetros de Consulta:</strong>
          <ul><li>accountNumber: Número da conta</li></ul>
        </li>
      </ul>   

<h3>Transferir entre contas</h3>
      <ul><strong>
        <li>URL: /api/accounts/transfer</li>
        <li>Método: POST</li>
        <li>Corpo da Requisição:</li></strong>
            <pre><code>
                  {
                    "sourceAccount": "string",
                    "destinationAccount": "string",
                    "amount": decimal
                  }
            </code></pre>
      </ul>
