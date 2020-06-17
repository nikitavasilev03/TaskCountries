# TaskCountries

DomainCore <br />
Тип: Class Library (.Net Core 3.1) <br />
Описание: для рыботы с БД<br />
Зависимости:<br />
  - Microsoft.EntityFrameworkCore
  - Microsoft.EntityFrameworkCore.SqlServer
  - Microsoft.EntityFrameworkCore.Tools

DataFillDB<br />
Тип: Console Application (.Net Core 3.1)<br />
Описание: скрипт для переноса данных в БД из RESTCountries<br />
Зависимости:<br />
  - RESTCountries.NET
  
TaskCountries<br />
Тип: WPF App (.Net Core 3.1)<br />
Описание: приложение для демонстрации выполненного задания<br />

CountiresDB.bak: бекап БД<br />

Для подключения к БД используется строка подключения в файле конфигурации (App.config)<br />
Так же есть возможность подключиться из приложения<br />
