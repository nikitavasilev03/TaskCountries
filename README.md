# TaskCountries

### Build application
Для сборки приложения необходим .NET Core 3.1 SDK!<br />
Способ 1:
 1. Открыть cmd или PowerShell b и перейти в папку ...\TaskCountries\TaskCountries
 2. Выполнить команду `dotnet run`<br/>
 
Способ 2:
 1. Открыть ...\TaskCountries\TaskCountries.sln через Visual Studio
 2. Сделать проект TaskCountries запускаемым по умолчанию
 3. Запустить проект

### Run application
Для запуска приложения необходим .NET Core 3.1 Runtime (если есть .NET Core SDK, то не надо)<br />
Запускаем TaskCountries.exe (...\TaskCountries\TaskCountries\bin\Debug(Realese)\netcoreapp3.1\TaskCountries.exe)

### Востановление БД
Для востановления БД предаставляю backup-файл MS SQL Server 14.0.1000:
 - CountiresDB.bak - текущий файл бэкапа

### Подключение к БД
Для подключения к БД используется строка подключения в файле конфигурации (connectionString="Data Source=...):<br/>
 - App.config для не собранного проекта (...\TaskCountries\TaskCountries\App.config)
 - TaskCountries.dll.config для собранного проекта (...\TaskCountries\TaskCountries\bin\Debug(Realese)\netcoreapp3.1\TaskCountries.dll.config)
Так же есть возможность подключиться из приложения, если заданая строка подключения через кофиг неверна<br />

### DomainCore
Тип: Class Library (.Net Core 3.1) <br />
Описание: для рыботы с БД<br />
Зависимости:<br />
  - Microsoft.EntityFrameworkCore
  - Microsoft.EntityFrameworkCore.SqlServer
  - Microsoft.EntityFrameworkCore.Tools
  
### TaskCountries
Тип: WPF App (.Net Core 3.1)<br />
Описание: приложение для демонстрации выполненного задания<br />
Зависимости:<br />
  - RESTCountries.NET
