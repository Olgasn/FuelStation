# FuelStation
Учебный пример простого Web приложения баз данных на языке C#, использующего такие технологии и инструменты как: 
ASP.NET Core 8, EF Core 8, Fluent Validation, XUnit.
Посмотреть - http://olas.tryasp.net/

Структура файла secrets.json, в котором хранятся данные для авторизации:
{
  "Database:login": "",
  "Database:password": ""
}

В этом файле, в соответвующих  местах, требуется указать имя пользователя и пароль для доступа к базе данных, размещенной на удаленном SQL Server. 

[![build and test](https://github.com/Olgasn/FuelStation/actions/workflows/build-and-test.yml/badge.svg?branch=identity)](https://github.com/Olgasn/FuelStation/actions/workflows/build-and-test.yml)

