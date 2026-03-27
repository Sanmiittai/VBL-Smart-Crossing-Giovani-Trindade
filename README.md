# VBL-Smart-Crossing-Giovani-Trindade

## Como Rodar o Mock da API

1. Baixe e instale o Mockoon: https://mockoon.com/
2. Abra o ambiente local com o arquivo `vbl-mock.json` incluído no repositório
3. Clique em "Start local server"
4. A API estará pronta para receber requests e rodar a simulação


## Como Rodar a Simulação

1. Baixe e instale a Unity: https://unity.com/
2. Adicione o projeto a partir da pasta `VBL-Smart-Crossing-Giovani-Trindade` dentro do repositório
3. Abra o projeto, se necessário instale a versão necessária da Unity
4. No projeto, abra a cena `VBL-Smart-Crossing` e clique em "Play" para iniciar a simulação

---

## Controles

| Tecla | Ação |
|---|---|
| W | Mover para a frente |
| S | Mover para trás |
| A | Mover para a esquerda |
| D | Mover para a direita |

---

## Fluxo da Simulação

1. Ao iniciar a cena, o `ApiManager` faz a request para a API, desserializa o JSON e envia os dados para o `GameManager`
2. `GameManager` atualiza as variáveis da simulação, envia as informações para o `HudManager` atualizar a HUD e permite o início da simulação
3. Baseado no intervalo de spawn dos carros, o `GameManager` pede para o `CarManager` spawnar carros
4. O `GameManager` constantemente atualiza as variáveis da simulação, baseado nas predições atuais e no seu tempo estimado para mudança
5. A cada novo nível, o `GameManager` atualiza as variáveis da simulação e o `ApiManager` faz uma nova requisição para pegar os dados do próximo nível
6. Ao ultrapassar o limite de tempo para passar de nível, ser atingido por um carro ou terminar todos os níveis, o jogador tem a opção de reiniciar a simulação

---

## Arquitetura

- A comunicação entre scripts é feita por um `EventManager` que centraliza diferentes eventos e ouvintes utilizando o Observer Pattern
- As principais variáveis que precisam ser compartilhadas entre classes utilizam um Scriptable Object para compartilhar o valor necessário, diminuindo o acoplamento das classes
- O máximo possível foi feito para deixar a simulação personalizável baseada em diversos parâmetros que podem mudar no futuro, para isso variáveis principais foram externalizadas para configuração e um sistema modular para criação de vias utilizando prefabs foi feito
- Utilização de uma pool de objetos para a criação dos carros, retirando a necessidade de instanciação do objeto em tempo real, apenas modificando os parâmetros dele para torná-lo ativo na simulação
