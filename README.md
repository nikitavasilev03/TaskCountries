# TaskCountries

DomainCore
Тип: Class Library (.Net Core 3.1)
Описание: для рыботы с БД
Зависимости:
  - Microsoft.EntityFrameworkCore
  - Microsoft.EntityFrameworkCore.SqlServer
  - Microsoft.EntityFrameworkCore.Tools

DataFillDB
Тип: Console Application (.Net Core 3.1)
Описание: скрипт для переноса данных в БД из RESTCountries
Зависимости:
  - RESTCountries.NET
  
TaskCountries
Тип: WPF App (.Net Core 3.1)
Описание: приложение для демонстрации выполненного задания

CountiresDB.bak: бекап БД

Для подключения к БД используется строка подключения в файле конфигурации (App.config)
Так же есть возможность подключиться из приложения 
