# Introdução 
Projeto para o desafio Hackaton do curso Pós Tech de Arquitetura de Sistemas .NET com Azure da FIAP, turma 3NETT

Esse projeto tem como objetivo criar um <u>Sistema de Agendamentos de Consultas Médicas</u> para a empresa <u>Health&Med</u>

# Detalhes da arquitetura

O sistema utiliza o SQLite para armazenar seus dados.

Decidimos por utilizar da arquitetura limpa para o projeto, devido à sua separação de preocupações e independência entre UI, regras de negócio e infraestrutura.

A solução é composta pelos seguintes projetos:

1. <b>Hackaton (API Web)</b><br>
    Camada responsável por controlar a interação Usuário/Sistema, recebendo os dados no formato necessário, orquestrando para onde devem ir, e devolvendo o resultado de forma amigável ao usuário.
2. <b>Hackaton.Application</b><br>
    Camada de aplicação que contem as interfaces para a comunicação entre os diferentes módulos do sistema.<br>
    Possui as regras de negócio (Use Cases) e Presenters para converter os resultados nos formatos de Output corretos.
3. <b>Hackaton.Domain</b><br>
    Projeto de Domínio, contendo as Entidades núcleo do negócio
4. <b>Hackaton.Data</b><br>
    Responsável pela comunicação entre Sistema e Repositório de dados, utilizando Migrations, Entity Framework e classes Repository
5. <b>Hackaton.Infraestructure</b><br>
    Contém os serviços de elementos externos ao negócio, como envio de email e geração do token de autenticação.

# Funcionalidades

As funcionalidades do sistema incluem:

1. <b>Cadastro do Usuário (Médico)</b><br>
    O médico deverá poder se cadastrar, preenchendo os campos obrigatórios: Nome, CPF, Número CRM, E-mail e Senha.
2. <b>Autenticação do Usuário (Médico)</b><br>
    O sistema deve permitir que o médico faça login usando o E-mail e uma Senha
3. <b>Cadastro/Edição de Horários Disponíveis (Médico)</b><br>
    O sistema deve permitir que o médico faça o Cadastro, Edição de seus dias e horários disponíveis para agendamento de consultas.
4. <b>Cadastro do Usuário (Paciente)</b><br>
    O paciente poderá se cadastrar preenchendo os campos: Nome, CPF, Email e Senha.
5. <b>Autenticação do Usuário (Paciente)</b><br>
    O sistema deve permitir que o paciente faça login usando o E-mail e Senha.
6. <b>Busca por Médicos (Paciente)</b><br>
    O sistema deve permitir que o paciente visualize a listagem dos médicos disponíveis.
7. <b>Agendamento de Consultas (Paciente)</b><br>
    Após selecionar o médico, o paciente deve visualizar os dias e horários disponíveis do médico.<br>
    O paciente poderá selecionar o horário de preferência e realizar o agendamento.
8. <b>Notificação de consulta marcada (Médico)</b><br>
    Após o agendamento, feito pelo usuário Paciente, o médico deverá receber um e-mail contendo:<br>
    Título do e-mail: <br>
    ”Health&Med - Nova consulta agendada” <br>
    Corpo do e-mail: <br>
    ”Olá, Dr. {nome_do_médico}! <br>
    Você tem uma nova consulta marcada! <br>
    Paciente: {nome_do_paciente}. <br>
    Data e horário: {data} às {horário_agendado}.” <br>

# Execução do Projeto
1. Instale o .NET 8.
2. Abra o projeto em uma IDE (Visual Studio Code ou Visual Studio 2022).
3. Configure o projeto Hackaton como o projeto de inicialização (Set as Start Project).
4. Abra o Package Manager Console e execute o comando Update-Database com o projeto TechChallenge5.Data selecionado.
5. Execute o projeto de API (dotnet run ou use o botão de start do Visual Studio).

# Tecnologias usadas no projeto
1. .NET 8
2. Entity Framework 8.0.8
3. SQLite
4. Dependency Injection
5. Swagger