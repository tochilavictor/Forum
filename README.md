# Forum
  В проекте реализована базовая функциональность форума.
Форум состоит из разделов, разделы из топиков, топики из сообщений.
Есть возможность зерегестрироваться и войти. Зарегестрированные пользователи 
деляться на пользователей, модераторов и администраторов.
<hr>
Незарегестрированные (незалогиненые) пользователи могут просматривать 
разделы форума, топики, и сообщения, просматривать профили пользователей,
но не могут оставлять сообщения(ответы) и создавать новые топики.
<hr>
Функциональность пользователя: функциональность незалогиненного пользователя + создание топиков,
добавление сообщений к уже существующим топикам, добавление ответа уже существующему сообщению,
просмотр и редактирования своего профиля(с возможностью загрузки аватара).
<hr>
Функциональность модератора: модерирует определённые разделы или раздел, т.е. функциональность 
пользователя + возможность редактирования(например перенести топик в другой раздел) и удаления
топиков раздела, в котором пользователь является модератором, + удаление сообщений в топиках (по тем же правилам)
<hr>
Функциональность администратора: функциональность пользователя + функциональность "модератора всех разделов",
+ управление пользователями (просмотр, назначение на роль модератора, удаление)