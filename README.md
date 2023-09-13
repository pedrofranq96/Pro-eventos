# Documentação do Projeto "Pro Eventos"

O projeto "Pro Eventos" é uma plataforma de eventos e palestrantes que permite aos usuários visualizar e comprar ingressos para palestras de tecnologia, bem como visualizar informações sobre palestrantes. Os usuários podem fazer login, editar seus perfis como palestrantes ou participantes e interagir com o sistema.

O projeto é dividido em duas partes principais: o backend e o frontend. O backend lida com a lógica de negócios, autenticação e gerenciamento de dados, enquanto o frontend é responsável pela interface do usuário.

## Backend

### Tecnologias/Dependências

As seguintes tecnologias e dependências foram usadas no backend do projeto:

- Microsoft.AspNetCore.Authentication.JwtBearer (v5.0.17): Middleware de autenticação baseado em JWT para ASP.NET Core.
- Microsoft.AspNetCore.Identity.EntityFrameworkCore (v5.0.17): Biblioteca para gerenciamento de identidade.
- Microsoft.AspNetCore.Mvc.NewtonsoftJson (v5.0.17): Serialização JSON usando Newtonsoft.Json.
- Microsoft.EntityFrameworkCore (v5.0.17): ORM (Object-Relational Mapping) para interagir com bancos de dados.
- Microsoft.EntityFrameworkCore.Design (v5.0.17): Ferramentas de design para Entity Framework Core.
- Microsoft.EntityFrameworkCore.SqlServer (v5.0.17): Suporte ao SQL Server para Entity Framework Core.
- Microsoft.EntityFrameworkCore.Tools (v5.0.17): Ferramentas adicionais para Entity Framework Core.
- Swashbuckle.AspNetCore (v5.6.3): Biblioteca para gerar documentação Swagger para APIs ASP.NET Core.

### Instalação do Backend

Para executar o backend em sua máquina, siga estas etapas:

1. Certifique-se de que você tem o [.NET SDK](https://dotnet.microsoft.com/download/dotnet/5.0) instalado em sua máquina.

2. Abra um terminal e navegue até a pasta do projeto backend.

3. Execute o seguinte comando para restaurar as dependências:

```bash
dotnet restore
```

4. Configure as variáveis de ambiente. Abra o arquivo `appsettings.json` na pasta `appsettings` e configure as informações do banco de dados SQL Server e a chave secreta JWT.

5. Execute o seguinte comando para aplicar as migrações e criar o banco de dados:

```bash
dotnet ef database update
```

6. Execute o seguinte comando para iniciar o servidor backend:

```bash
dotnet run
```

O servidor estará disponível em `http://localhost:5000`.

## Frontend

### Tecnologias/Dependências

As seguintes tecnologias e dependências foram usadas no frontend do projeto:

- Angular (v12.2.0): Uma estrutura de aplicativo para construir aplicativos da web.
- Bootstrap (v4.6.2) e Bootswatch (v5.3.1): Estilos e temas para a interface do usuário.
- Ngx-Bootstrap (v8.0.0): Componentes do Bootstrap para Angular.
- Ngx-Currency (v2.5.2): Uma biblioteca para formatação de campos de moeda.
- Ngx-Spinner (v12.0.0): Um componente de spinner para indicar carregamento.
- Ngx-Toastr (v14.3.0): Uma biblioteca para exibir notificações Toast.
- RxJS (v6.6.0): Biblioteca para programação reativa em JavaScript.

### Instalação do Frontend

Para executar o frontend em sua máquina, siga estas etapas:

1. Certifique-se de que você tem o [Node.js](https://nodejs.org/) instalado em sua máquina.

2. Abra um terminal e navegue até a pasta do projeto frontend.

3. Execute o seguinte comando para instalar as dependências:

```bash
npm install
```

4. Após a conclusão da instalação, inicie o servidor de desenvolvimento com o seguinte comando:

```bash
ng serve
```

5. O aplicativo estará disponível em `http://localhost:4200` em seu navegador. Você pode acessar a interface do usuário do projeto a partir deste URL.

## Uso da Aplicação

Acesse a interface do usuário do frontend em `http://localhost:4200` em seu navegador. Você pode criar uma conta de usuário, fazer login, visualizar eventos, palestrantes, comprar ingressos e editar seu perfil como palestrante ou participante.

Lembre-se de que esta é apenas uma documentação inicial, e o projeto pode ter atualizações futuras. Certifique-se de consultar a documentação específica das tecnologias e bibliotecas usadas para obter mais informações e personalizações.

Para qualquer dúvida ou problema, entre em contato com a equipe de desenvolvimento do projeto.
