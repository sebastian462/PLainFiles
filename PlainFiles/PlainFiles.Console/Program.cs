using PlainFiles.Core;
using System.Xml.Linq;
using System.Globalization;


using var logger = new LogWriter(".\\app.log");
logger.WriteLog("INFO", "aplicacion iniciada.");

List<User> users = new();

if (!File.Exists("Users.txt"))
{
    File.WriteAllText("Users.txt",
@"jzuluaga,P@ssw0rd123!,true
mbedoya,S0yS3gur02025*,false");
}
foreach (var line in File.ReadAllLines("Users.txt"))
{
    var parts = line.Split(',');
    users.Add(new User(parts[0], parts[1], bool.Parse(parts[2])));
}
User? loggedUser = null;
int attempts = 0;
string at = string.Empty;


while (attempts < 3 && loggedUser == null)
{
    Console.Write("Usuario: ");
    string username = Console.ReadLine() ?? string.Empty;
    logger.WriteLog("INFO", $" Ingreso el usuario. {username}");
    at = username;
    Console.Write("Contraseña: ");
    string password = Console.ReadLine() ?? string.Empty;
    logger.WriteLog("INFO", $"Contraseña del usuario. {password}");

    var found = users.FirstOrDefault(u => u.Username == username);

    if (found == null)
    {
        Console.WriteLine("Usuario no encontrado.\n");
        attempts++;
        continue;
    
    }

    if (!found.Active)
    {
        Console.WriteLine("Usuario BLOQUEADO.");
        logger.WriteLog("INFO", $"El usuario supero el limite de intentos. {username}");

        return;
    }

    if (found.Password == password)
    {
        loggedUser = found;
        Console.WriteLine($"Bienvenido: {found.Username}");
        Console.WriteLine();
    }
    else 
    {
        Console.WriteLine("Contraseña Incorrecta.\n");
        logger.WriteLog("INFO", $" Ingreso mal la contraseña, el usuario. {username}");
        attempts++;
    } 
}

if (loggedUser == null)
{
    Console.WriteLine("Usuario bloqueado por intentos fallidos.");
    var userToBlock = users.First(u => u.Username == at);
    userToBlock.Active = false;


    File.WriteAllLines("Users.txt", users.Select(u => $"{u.Username},{u.Password},{u.Active}"));
    return;
}


Console.Write("Digite el nombre de la lista: ");
var listName = Console.ReadLine();


var manualCsv = new ManualCsvHelper();
var people = manualCsv.ReadCsv($"{listName}.csv");
var option = string.Empty;

do 
{
    option = MyMenu();
    Console.WriteLine();
    switch (option)
    {
        case "1":
            AddPerson();
            break;

        case "2":
            ListPeople();
            break;

        case "3":
            SaveFile(people, listName);
            Console.WriteLine("Archivo guardado.");
            logger.WriteLog("INFO", $"El usuario {loggedUser?.Username} Guardo los cambios.");
            break;

        case "4":
            Delete();
            break;

        case "5":
            SortData ();
            break;

        case "6":
            EditUser(people);
            break;

        case "7":
            TotalBalance(people);
            break;

        case "0":
            Console.WriteLine("Saliendo...");
            logger.WriteLog("INFO", $"El usuario {loggedUser?.Username} Finalizo el programa");
            break;
        default:
            Console.WriteLine("Opción no válida.");
            logger.WriteLog("INFO", $"El usuario {loggedUser?.Username} Ingreso una opcion no valida.");
            break;

    }

} while (option != "0");



void SortData()
{

    int order;
    do
    {
        logger.WriteLog("INFO", $"El usuario {loggedUser?.Username} quiere ordenar la lista.");
        Console.Write("Por cual campo desea ordenar por: 0. Nombre, 1. Apellido, 2. ");
        var orderString = Console.ReadLine();
        int.TryParse(orderString, out order);
        if (order < 0 || order > 2)
        {
            Console.WriteLine("Orden no valido. Intente de nuevo.");
        }

    } while (order < 0 || order > 2);

    int type;
    do

    {
        Console.Write("Desea ordenar 0. Ascendente, 1. Descendente? ");
        var typeString = Console.ReadLine();
        int.TryParse(typeString, out type);
        if (type < 0 || type > 2)
        {
            Console.WriteLine("Tipo de orden no valido. Intente de nuevo.");
        }

    }while (order < 0 || order > 2);

    people.Sort((a, b) =>
    {
        int cmp;
        if (order == 2)
        {
            bool parsedA = int.TryParse(a[2], out var ageA);
            bool parsedB = int.TryParse(b[2], out var ageB);

            if (!parsedA) ageA = int.MinValue;
            if (!parsedB) ageB = int.MinValue;

            cmp = ageA.CompareTo(ageB);
        }
        else
        {
            cmp = string.Compare(a[order], b[order], StringComparison.OrdinalIgnoreCase);
        }

        return type == 0 ? cmp : -cmp;
    });

    Console.WriteLine("Lista ordenada: ");

}

void ListPeople()
{
    logger.WriteLog("INFO", $"El usuario {loggedUser?.Username} quiere listar las personas.");
    Console.WriteLine("===========================================");
    Console.WriteLine("Lista de personas: ");

    int index = 1;

    foreach (var person in people)
    {
        Console.WriteLine($"{index}");

        Console.WriteLine($"   Nombre:     {person[1]}");
        Console.WriteLine($"   Apellido:   {person[2]}");
        Console.WriteLine($"   Teléfono:   {person[3]}");
        Console.WriteLine($"   Balance:    {person[4]}");
        index++;
        Console.WriteLine();
    }

    Console.WriteLine("===========================================");
}

void AddPerson()
{
    logger.WriteLog("INFO", $"El usuario {loggedUser?.Username} quiere añadir un nuevo nombre");

    int id;
    string? idInput;
    do
    {
        Console.Write("Ingrese ID (numero positivo): ");
        idInput = Console.ReadLine();

        if (!Validator.ValidId(idInput, out id))
        {
            Console.WriteLine("ID invalido. Intente de nuevo");
            continue;
        }

        if (people.Any(p => p[0] == id.ToString()))
        {
            Console.WriteLine("Esye ID ya existe. Debe de ser unico");
            id = -1;
        }

    } while (id <= 0);

    string? name;

    do
    {
        Console.Write("Digite el nombre: ");
        name = Console.ReadLine();
        logger.WriteLog("INFO", $"{loggedUser.Username} ingreo el nombre: {name}");

        if (!Validator.ValidName(name))
        {
            Console.WriteLine("Nombre invalido.");
        }

    } while (!Validator.ValidName(name));

    string? lastName;
    do
    {
        Console.Write("Digite el apellido: ");
        lastName = Console.ReadLine();
        logger.WriteLog("INFO", $"{loggedUser.Username} ingreso el apellido: {lastName}");

        if (!Validator.ValidName(lastName))
        {
            Console.WriteLine("Apellido invalido.");
        }
    } while (!Validator.ValidName(lastName));



    string? phone;
    do
    {
        Console.Write("Digite el teléfono: ");
        phone = Console.ReadLine();

        if (!Validator.ValidPhone(phone))
            Console.WriteLine(" Teléfono inválido.");
    } while (!Validator.ValidPhone(phone));

    decimal balance;
    string? balanceInput;


    do
    {
        Console.Write("Digite el saldo inicial: ");
        balanceInput = Console.ReadLine();

        if (!Validator.ValidBalance(balanceInput, out balance))
            Console.WriteLine("El saldo debe ser un número positivo.");
    } while (!Validator.ValidBalance(balanceInput, out balance));


    people.Add(new string[] { id.ToString(), name ?? string.Empty, lastName ?? string.Empty, phone ?? string.Empty, balance.ToString()});
    logger.WriteLog("INFO", $"{loggedUser?.Username} añadió a {name} {lastName} con ID {id}");
}

void Delete()
{
    logger.WriteLog("INFO", $"El usuario {loggedUser?.Username} quiere eliminar un registro");
    Console.Write("Digite el nombre: ");
    var nameToDelete = Console.ReadLine();
    logger.WriteLog("INFO", $"{loggedUser.Username} quiere eliminar el registro de: {nameToDelete}");
    var peopleToDelete = people
        .Where(p => p[1].Equals(nameToDelete, StringComparison.OrdinalIgnoreCase))
        .ToList();

    if (peopleToDelete.Count == 0)
    {
        Console.WriteLine("No se encontraron personas con ese nombre.");
        return;
    }

    for (int i = 0; i < peopleToDelete.Count; i++)
    {
        Console.WriteLine($"ID: {i} - Nombres: {peopleToDelete[i][0]} {peopleToDelete[i][1]}");
    }
    int id;

    do
    {
        Console.Write("Digite la opcion (0) si desea borrar, o (-1) para cancelar? ");
        logger.WriteLog("INFO", $"{loggedUser.Username} Elimino a : {nameToDelete}");

        var idString = Console.ReadLine();
        int.TryParse(idString, out id);
        if (id < -1 || id > peopleToDelete.Count)
        {
            Console.WriteLine("ID no válido. Intente de nuevo.");
        }
    } while (id < -1 || id > peopleToDelete.Count);

    if (id == -1)
    {
        Console.WriteLine("Operacion cancelada.");
        return;
    }
    var personToRemove = peopleToDelete[id];
    people.Remove(personToRemove);
}


void EditUser(List<string[]> people)
{
    logger.WriteLog("INFO", $"El usuario {loggedUser?.Username} abrió EditUser");

    Console.Write("Ingrese el ID de la persona a editar: ");
    var idStr = Console.ReadLine() ?? string.Empty;

    var person = people.FirstOrDefault(p => p[0] == idStr);

    if (person == null)
    {
        Console.WriteLine("El ID no existe.");
        return;
    }

    Console.WriteLine("\n--- EDITAR PERSONA ---");
    Console.WriteLine("Presione ENTER para mantener el valor actual.\n");

    // Nombre
    Console.Write($"Nombre actual ({person[1]}): ");
    var newName = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(newName) && Validator.ValidName(newName))
        person[1] = newName;

    // Apellido
    Console.Write($"Apellido actual ({person[2]}): ");
    var newLast = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(newLast) && Validator.ValidName(newLast))
        person[2] = newLast;


    // Teléfono
    Console.Write($"Teléfono actual ({person[3]}): ");
    var newPhone = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(newPhone) && Validator.ValidPhone(newPhone))
        person[3] = newPhone;

    // Balance
    Console.Write($"Balance actual ({person[4]}): ");
    var newBal = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(newBal) && Validator.ValidBalance(newBal, out var bal))
        person[4] = bal.ToString();

    Console.WriteLine("\nPersona actualizada correctamente.\n");
}


void TotalBalance(List<string[]> people)
{
    Console.WriteLine("\n--- LISTA DE PERSONAS ---\n");

    decimal totalBalance = 0;

    foreach (var p in people)
    {
        Console.WriteLine("===========================================");
        Console.WriteLine();
        Console.WriteLine($"   Nombre:     {p[1]}");
        Console.WriteLine($"   Apellido:   {p[2]}");
        Console.WriteLine($"   Teléfono:   {p[3]}");
        Console.WriteLine($"   Balance:    {p[4]}");
        Console.WriteLine("===========================================");

        if (decimal.TryParse(p[4], out var bal))
            totalBalance += bal;
    }

    Console.WriteLine($"Balance TOTAL: ${totalBalance.ToString("N2", new CultureInfo("es-CO"))}");
}




string MyMenu()
{
    Console.WriteLine();
    Console.WriteLine("1. Adiccionar.");
    Console.WriteLine("2. Mostrar.");
    Console.WriteLine("3. Grabar.");
    Console.WriteLine("4. Eliminar.");
    Console.WriteLine("5. Ordenar.");
    Console.WriteLine("6. Editar Usuario.");
    Console.WriteLine("7. Sumar Balance.");
    Console.WriteLine("0. Salir.");
    Console.WriteLine("Seleccione una opcion: ");
    return Console.ReadLine() ?? string.Empty;

}
SaveFile(people, listName);

void SaveFile(List<string[]> people, string? listName)

{
    manualCsv.WriteCsv($"{listName}.csv", people);
}



