# FuelStation
Учебный пример простого Web приложения баз данных на языке C#, использующего такие технологии и инструменты как: 
ASP.NET Core 10, EF Core 10, Fluent Validation, XUnit.

Посмотреть работу на внешнем хостинге - http://olas.tryasp.net/

Структура файла secrets.json, в котором хранятся данные для авторизации:

{
  "Database:login": "",
  "Database:password": ""
}

В этом файле, в соответвующих  местах, требуется указать имя пользователя и пароль для доступа к базе данных, размещенной на удаленном SQL Server. 

Статус:

[![build and test](https://github.com/Olgasn/FuelStation/actions/workflows/build-and-test.yml/badge.svg?branch=identity)](https://github.com/Olgasn/FuelStation/actions/workflows/build-and-test.yml)



