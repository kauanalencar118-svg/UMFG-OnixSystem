OnixSystem_MVC/
│
├── wwwroot/                # Arquivos estáticos do front-end
│   ├── css/                # Estilização das páginas
│   ├── images/             # Imagens e ícones
│   └── js/                 # Scripts JavaScript
│
├── Controllers/            # Controladores das requisições
│
├── Data/                   # Configurações e acesso ao banco
│   ├── Contexts/           # Contexto do Entity Framework Core
│   └── Repositorios/       # Operações de acesso aos dados
│
├── Models/                 # Entidades, validações e regras de negócio
│
├── Migrations/             # Histórico de alterações do banco de dados
│
├── Views/                  # Telas da aplicação em Razor
│   ├── Pages/              # Organização das páginas do sistema
│   ├── Shared/             # Layouts e componentes compartilhados
│   └── Partials/           # Trechos reutilizáveis das telas
│       └── Layout/         # Componentes visuais do layout
│
├── appsettings.json        # Configurações da aplicação e conexão com o banco
├── Program.cs              # Inicialização e configuração da aplicação
└── README.md               # Documentação do projeto