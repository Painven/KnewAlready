# KnewAlready API
*API для скрытого обмена запросами на действие, которые не будут известны до момента пока оба человека не согласятся на них*
## Обязательные требования
Что бы запрос был корректно выполнен (подтвержден) другим человеком должны совпасть все 3 условия
1) В качестве подтверждающего точно указан *никнейм* пользователя на сервисе с которым вы хотите иметь дело
2) В качестве категории у вас обоих указана *категория* с точным совпадением у вас обоих (например "Футбол", "Поездка за продуктами", "HOTA")
3) Вы попали в временной интервал, который указал человек *первым* создавший запрос, отсчитываемый от момента создания запроса
Далее при отправке запроса получается 2 возможных варианта
  1 Вариант - Тот с кем вы имеете дело не отправлял запроса (или он уже истек) - в этом случае **вы становитесь иницитором**
  2 Вариант - Тот кому вы отправляете запрос уже создал его и ожидает вашего подтверждения, - в этом случае **ваш запрос является подтверждением**. Вы сами и тот кто уже вас ожидает будет уведомлен (по тем каналам связи которые указаны в настройка - Telegram, Email), что событие подтверждено
## Пояснение смысла работы
### Абстрактный пример: 
##### Вам нужно от человека подтверждение, что вы готовы выполнить какое то действие. Но только в том случае, если **согласны вы _оба_** 
##### Если согласна только одна из сторон, то просто ничего не произойдет. И **другая сторона не будет знать**, что вы вообще соглашались выполнять это событие
### Конкретный пример [1] Игра HotA
В игре HOTA вы можете предложить сопернику перегенерировать карту (если у вас не осталось рестартов, обычно их 1 или 2), но соперник может отказаться, и в этом случае он узнает что у вас плохое положение.
В тоже время, если делать это в скрытом виде - через этот API, то получается что только когда вы **оба** согласитесь на перегенерацию вам обоим придет уведомление что событие **произошло**.
В ином случае - когда на перегенерацию согласился только один из вас, ничего не произойдет, и все просто будет идти своим чередом.
Тоесть, еще раз. Если например: у вас обоих "плохой респ" и вы <strong>ОБА</strong> не хотите его играть, но и говорить сопернику об этом "стесняетесь" (т.к. он с большой вероятностью откажет, и будет прав) - в этом случае можно использовать данный API
### Конкретный пример [2] IRL - футбол
Вы переодически ходите играть в футбол со знакомым(ой), но не хотите напрашиватся
И вот, вы можете разместить запрос и если оба согласны - вы пойдете играть оба
Если кто то не согласен, - он может пойти играть с другими один, или не пойти вообще (возможно, например, он хотел пойти только в том случае, если только пошли вы)
### Конкретный пример [3] IRL - личная жизнь
Пусть например вы оба увлекатесь картингом, но просить каждый раз - вам не хочется (надоедать человеку). И вот тут тоже можно сделать скрытый запрос
Когда этого захотят оба человека вы оба узнаете и пойдете на картинг/любое другое действие, не важно что
