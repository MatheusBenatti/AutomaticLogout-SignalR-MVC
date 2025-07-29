Controle de Sessões com SignalR no ASP.NET MVC

🎯 Objetivo
Este projeto é uma demonstração de como controlar sessões de usuário com SignalR em uma aplicação ASP.NET MVC, focando na funcionalidade de logout automático quando todas as abas do navegador de um usuário são fechadas.

🚀 Tecnologias Utilizadas
Framework: .NET Framework 4.8
Framework MVC: ASP.NET MVC 5
WebSockets: SignalR Classic (Microsoft.AspNet.SignalR)
Autenticação: OAuth (para o processo de autenticação) e Cookies (para gerenciar a sessão autenticada)
Gerenciamento de Estado: Sessões (para controle de login do usuário)
Frontend: JavaScript (para interação com SignalR)

💡 Como Funciona
Registro de Abas Ativas: Cada aba ativa do navegador de um usuário é registrada via SignalR.
Desconexão por Fechamento de Aba: Ao fechar uma aba, a conexão SignalR correspondente é automaticamente terminada.
Logout Automático: Se todas as abas do navegador de um usuário forem fechadas (ou seja, a última aba ativa é desconectada), o sistema realiza o logout automático do usuário.
Controle de Conexões: O processo de logout é gerenciado pela contagem do número de conexões SignalR ativas por usuário.

📖 Uso
Após executar o projeto, a aplicação será aberta no seu navegador padrão.
Faça Login: O projeto possui uma tela de login simples para simular um usuário autenticado.
Abra Múltiplas Abas: Abra o mesmo endereço da aplicação em várias abas ou janelas do navegador.
Teste o Logout:
Feche uma aba por vez e observe que o usuário permanece logado enquanto houver outras abas abertas.
Feche a última aba aberta e observe que o sistema deve deslogar o usuário automaticamente.
