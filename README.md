# Controle de Sessões com SignalR no ASP.NET MVC

Este é um projeto de demonstração utilizando as seguintes tecnologias:

- **.NET Framework 4.8**
- **ASP.NET MVC 5**
- **SignalR Clássico** (`Microsoft.AspNet.SignalR`)
- **Session e Cookies** para controle de login do usuário

## Objetivo

Testar a funcionalidade de logout automático de um usuário quando **todas as abas do navegador forem fechadas**.

## Funcionalidades

- Cada aba ativa de um usuário é registrada via SignalR.
- Quando uma aba é fechada, a conexão SignalR é encerrada.
- Se nenhuma aba estiver mais aberta (última aba fechada), o sistema realiza o logout automático do usuário.
- O controle é feito via contagem de conexões por usuário.

## Como funciona

- O `Hub` do SignalR identifica o usuário por meio de `QueryString`.
- Utiliza um dicionário compartilhado para contar conexões abertas por usuário.
- No evento `OnDisconnected`, verifica se o número de abas abertas caiu para 0.
- Se sim, chama o serviço de logout (Session/Cookies são limpos).

## Observação

Durante um **refresh (F5)** da aba, o SignalR desconecta e reconecta rapidamente. Por isso, foi colocado um pequeno tempo de tolerância para teste
