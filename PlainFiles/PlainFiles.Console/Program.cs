using PlainFiles.Core;

Console.Write("Digite el nombre de la lista: ");
var listName = Console.ReadLine();


var manualCsv = new ManualCsvHelper();
var people = manualCsv.ReadCsv($"{listName}.csv");
var option = string.Empty;

do 
{
    option = MyMenu();
    Console.ReadLine();
    Console.ReadLine();
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
            break;

        case "4":
            Delete();
            break;

        case "5":
            SortData ();
            break;

        case "0":
            Console.WriteLine("Saliendo...");
            break;
        default:
            Console.WriteLine("Opción no válida.");
            break;

    }

} while (option != "0");



void SortData()
{

    int order;
    do
    {
        Console.Write("Por cual campo desea ordenar por: 0. Nombre, 1. Apellido, 2. Edad? ");
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
    Console.WriteLine("Lista de personas: ");


    Console.WriteLine($"Nombres|Apellidos|Edad");
    foreach (var person in people)
    {
        Console.WriteLine($"{person[0]}|{person[1]}|{person[2]}");
    }
}

void AddPerson()
{
    Console.Write("Digite el nombre: ");
    var name = Console.ReadLine();
    Console.Write("Digite el apellido: ");
    var lastName = Console.ReadLine();
    Console.Write("Digite la edad: ");
    var age = Console.ReadLine();
    people.Add(new string[] { name ?? string.Empty, lastName ?? string.Empty, age ?? string.Empty });
}

void Delete()
{
    Console.Write("Digite el nombre: ");
    var nameToDelete = Console.ReadLine();
    var peopleToDelete = people
        .Where(p => p[0].Equals(nameToDelete, StringComparison.OrdinalIgnoreCase))
        .ToList();

    if (peopleToDelete.Count == 0)
    {
        Console.WriteLine("No se encontraron personas con ese nommbre.");
        return;
    }

    for (int i = 0; i < peopleToDelete.Count; i++)
    {
        Console.WriteLine($"ID: {i} - Nombres: {peopleToDelete[i][0]} {peopleToDelete[i][1]}, Edad: {peopleToDelete[i][2]}");
    }
    int id;

    do
    {
        Console.Write("Digite el ID del elemento que desea borrar, 0 -1 para cancelar? ");
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

string MyMenu()
{
    Console.ReadLine();
    Console.WriteLine("1. Adiccionar.");
    Console.WriteLine("2. Mostrar.");
    Console.WriteLine("3. Grabar.");
    Console.WriteLine("4. Eliminar.");
    Console.WriteLine("5. Ordenar.");
    Console.WriteLine("0. Salir.");
    Console.WriteLine("Seleccione una opcion: ");
    return Console.ReadLine() ?? string.Empty;

}
SaveFile(people, listName);

void SaveFile(List<string[]> people, string? listName)

{
    manualCsv.WriteCsv($"{listName}.csv", people);
}



