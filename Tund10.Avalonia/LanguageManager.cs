using System;
using System.Collections.Generic;
using System.IO;

namespace Tund10.Avalonia;

public static class LanguageManager
{
    private static string _currentLanguage = "EE";
    private static readonly string _settingsPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "AutoApp_Language.txt");

    public static Dictionary<string, Dictionary<string, string>> Translations = new()
    {
        ["EE"] = new()
        {
            ["Title"] = "Auto Register",
            ["Login"] = "Logi sisse",
            ["Username"] = "Kasutajanimi",
            ["Password"] = "Parool",
            ["Search"] = "Otsi...",
            ["Owners"] = "OMANIKUD",
            ["Cars"] = "AUTOD",
            ["ServicesLogs"] = "TEENUSED & LOGID",
            ["ID"] = "ID",
            ["Name"] = "Nimi",
            ["Phone"] = "Telefon",
            ["Actions"] = "Tegevus",
            ["AddCar"] = "Lisa auto",
            ["Update"] = "Uuenda",
            ["Delete"] = "Kustuta",
            ["Add"] = "Lisa",
            ["Brand"] = "Bränd",
            ["Model"] = "Mudel",
            ["RegNumber"] = "Reg Number",
            ["Owner"] = "Omanik",
            ["AddService"] = "Lisa teenus",
            ["ServiceName"] = "Teenus Nimi",
            ["Price"] = "Hind",
            ["Car"] = "Auto",
            ["Service"] = "Teenus",
            ["Date"] = "Kuupäev",
            ["Mileage"] = "Kilomeetrid",
            ["Save"] = "Salvesta",
            ["Cancel"] = "Tühista",
            ["Message"] = "Sõnum",
            ["OK"] = "OK",
            ["Welcome"] = "Tere tulemast, admin!",
            ["WrongCredentials"] = "Vale kasutajanimi või parool!",
            ["FillAllFields"] = "Palun täitke kõik väljad.",
            ["CarExists"] = "Sellise registreerimisnumbriga auto on juba olemas.",
            ["CarAdded"] = "Auto lisati edukalt!",
            ["OwnerExists"] = "See omanik on juba olemas.",
            ["OwnerAdded"] = "Omanik lisati edukalt!",
            ["OwnerUpdated"] = "Omanik uuendati edukalt!",
            ["OwnerDeleted"] = "Omanik on edukalt kustutatud.",
            ["CannotDeleteOwner"] = "Seda omanikku ei saa kustutada. Eemalda esmalt seotud autod.",
            ["CarUpdated"] = "Auto uuendati edukalt!",
            ["CarDeleted"] = "Auto on edukalt kustutatud!",
            ["CannotDeleteCar"] = "Seda autot ei saa kustutada. Kustuta esmalt teeninduslogid.",
            ["ServiceAdded"] = "Teenus lisati edukalt!",
            ["ServiceUpdated"] = "Teenus uuendati edukalt!",
            ["ServiceDeleted"] = "Teenus kustutati edukalt!",
            ["ServiceExists"] = "See teenus on juba olemas.",
            ["CannotDeleteService"] = "Seda teenust ei saa kustutada. Eemalda esmalt seotud autoteeninduse logid.",
            ["LogDeleted"] = "Hooldusandmed kustutati edukalt!",
            ["LogExists"] = "Selle kirje jaoks on sellel kuupäeval juba olemas logi.",
            ["InvalidMileage"] = "Lubamatu läbisõidu väärtus.",
            ["InvalidPrice"] = "Palun sisestage kehtiv positiivne hind.",
            ["UpdateOwner"] = "Uuenda omanik",
            ["UpdateCar"] = "Uuenda auto",
            ["UpdateService"] = "Uuenda teenus",
            ["AddServiceTitle"] = "Lisa teenus",
            ["SelectService"] = "Vali teenus",
            ["AddCarBtn"] = "Lisa auto",
            ["UpdateBtn"] = "Uuenda",
            ["DeleteBtn"] = "Kustuta",
            ["AddServiceBtn"] = "Lisa teenus"
        },
        ["RU"] = new()
        {
            ["Title"] = "Автомобильный регистр",
            ["Login"] = "Войти",
            ["Username"] = "Имя пользователя",
            ["Password"] = "Пароль",
            ["Search"] = "Поиск...",
            ["Owners"] = "ВЛАДЕЛЬЦЫ",
            ["Cars"] = "АВТОМОБИЛИ",
            ["ServicesLogs"] = "УСЛУГИ И ЛОГИ",
            ["ID"] = "ID",
            ["Name"] = "Имя",
            ["Phone"] = "Телефон",
            ["Actions"] = "Действия",
            ["AddCar"] = "Добавить авто",
            ["Update"] = "Обновить",
            ["Delete"] = "Удалить",
            ["Add"] = "Добавить",
            ["Brand"] = "Марка",
            ["Model"] = "Модель",
            ["RegNumber"] = "Рег. номер",
            ["Owner"] = "Владелец",
            ["AddService"] = "Добавить услугу",
            ["ServiceName"] = "Название услуги",
            ["Price"] = "Цена",
            ["Car"] = "Авто",
            ["Service"] = "Услуга",
            ["Date"] = "Дата",
            ["Mileage"] = "Пробег",
            ["Save"] = "Сохранить",
            ["Cancel"] = "Отмена",
            ["Message"] = "Сообщение",
            ["OK"] = "ОК",
            ["Welcome"] = "Добро пожаловать, admin!",
            ["WrongCredentials"] = "Неверное имя пользователя или пароль!",
            ["FillAllFields"] = "Пожалуйста, заполните все поля.",
            ["CarExists"] = "Автомобиль с таким регистрационным номером уже существует.",
            ["CarAdded"] = "Автомобиль успешно добавлен!",
            ["OwnerExists"] = "Этот владелец уже существует.",
            ["OwnerAdded"] = "Владелец успешно добавлен!",
            ["OwnerUpdated"] = "Владелец успешно обновлен!",
            ["OwnerDeleted"] = "Владелец успешно удален.",
            ["CannotDeleteOwner"] = "Невозможно удалить этого владельца. Сначала удалите связанные автомобили.",
            ["CarUpdated"] = "Автомобиль успешно обновлен!",
            ["CarDeleted"] = "Автомобиль успешно удален!",
            ["CannotDeleteCar"] = "Невозможно удалить этот автомобиль. Сначала удалите журналы обслуживания.",
            ["ServiceAdded"] = "Услуга успешно добавлена!",
            ["ServiceUpdated"] = "Услуга успешно обновлена!",
            ["ServiceDeleted"] = "Услуга успешно удалена!",
            ["ServiceExists"] = "Эта услуга уже существует.",
            ["CannotDeleteService"] = "Невозможно удалить эту услугу. Сначала удалите связанные журналы обслуживания.",
            ["LogDeleted"] = "Данные обслуживания успешно удалены!",
            ["LogExists"] = "Запись для этой даты уже существует.",
            ["InvalidMileage"] = "Недопустимое значение пробега.",
            ["InvalidPrice"] = "Пожалуйста, введите действительную положительную цену.",
            ["UpdateOwner"] = "Обновить владельца",
            ["UpdateCar"] = "Обновить автомобиль",
            ["UpdateService"] = "Обновить услугу",
            ["AddServiceTitle"] = "Добавить услугу",
            ["SelectService"] = "Выберите услугу",
            ["AddCarBtn"] = "Добавить авто",
            ["UpdateBtn"] = "Обновить",
            ["DeleteBtn"] = "Удалить",
            ["AddServiceBtn"] = "Добавить услугу"
        }
    };

    public static string CurrentLanguage
    {
        get => _currentLanguage;
        set
        {
            _currentLanguage = value;
            SaveLanguage();
        }
    }

    public static string Get(string key)
    {
        if (Translations.ContainsKey(_currentLanguage) && Translations[_currentLanguage].ContainsKey(key))
            return Translations[_currentLanguage][key];
        return key;
    }

    public static void LoadLanguage()
    {
        if (File.Exists(_settingsPath))
        {
            _currentLanguage = File.ReadAllText(_settingsPath).Trim();
        }
    }

    private static void SaveLanguage()
    {
        File.WriteAllText(_settingsPath, _currentLanguage);
    }
}
