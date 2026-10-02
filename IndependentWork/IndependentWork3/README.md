# Самостійна робота №3
### __Тема:__ Аналіз інкапсуляції в open-source проєктах. Практики валідації полів.

# __Звіт з аналізу інкапсуляції в Open-Source проєкті__

## 1. Обраний проєкт
- **Назва:** ASP.NET Core
- **Посилання на GitHub:** https://github.com/dotnet/aspnetcore

## 2. Аналіз інкапсуляції
### Клас: `HttpContext`
- **Опис класу:** HttpContext — публічний абстрактний клас, який представляє контекст HTTP-запиту та відповіді.

- **Поля:** У класі HttpContext немає звичайних публічних полів. Для приховування внутрішніх даних використовується приватне поле _context у вкладеному класі HttpContextDebugView.
```csharp
private readonly HttpContext _context = context;
```
- **Властивості:** Клас містить властивості тільки для читання (get), наприклад Request, Response, Connection та Features, а також властивості для читання і зміни (get; set;), наприклад User, Items, RequestServices, TraceIdentifier та Session.
```csharp
public abstract HttpRequest Request { get; } 
public abstract HttpResponse Response { get; } 
public abstract ClaimsPrincipal User { get; set; }
```
Автоматичних властивостей у наведених прикладах немає.
- **Індексатори/Оператори:** Індексаторів та перевантажених операторів у класі HttpContext немає.

### Клас: 'HttpContextDebugView'
- **Опис класу:** HttpContextDebugView — вкладений приватний клас, який використовується для представлення даних HttpContext під час налагодження. Він оголошений як private sealed, тому доступний лише всередині HttpContext і не може мати похідних класів.
- **Поля:** Клас має приватне поле _context з модифікатором readonly, тому до нього немає прямого доступу ззовні, а змінити його після створення не можна
```csharp
private readonly HttpContext _context = context;
```
- **Властивості:** Клас має властивості тільки для читання, які повертають дані з _context.
```csharp
public HttpRequest Request => _context.Request; 
public HttpResponse Response => _context.Response;
```
- **Індексатори/Оператори:** Індексаторів та перевантажених операторів у класі немає.

### Клас: `KestrelServerLimits`
- **Опис класу:** KestrelServerLimits — клас, який містить налаштування та обмеження для HTTP-сервера Kestrel. Він використовується для керування такими параметрами, як максимальний розмір буфера відповіді та максимальна кількість заголовків запиту.

- **Файл:** https://github.com/dotnet/aspnetcore/blob/main/src/Servers/Kestrel/Core/src/KestrelServerLimits.cs?utm_source=chatgpt.com

- **Поля:** Клас використовує приватні поля для зберігання значень властивостей, наприклад _maxResponseBufferSize та _maxRequestHeaderCount. Це приховує внутрішні дані класу та забезпечує доступ до них через властивості.
```csharp
private long? _maxResponseBufferSize = 64 * 1024;
private int _maxRequestHeaderCount = 100;
```
- **Властивості:** Клас має властивості з get та set. У set деяких властивостей виконується перевірка значення перед його збереженням у приватному полі. Наприклад, MaxResponseBufferSize та MaxRequestHeaderCount перевіряють коректність переданих значень.
```csharp
public long? MaxResponseBufferSize 
{
     get => _maxResponseBufferSize; 
     set 
     {
        if (value.HasValue && value.Value < 0) 
        {
            throw new ArgumentOutOfRangeException(nameof(value), CoreStrings.NonNegativeNumberOrNullRequired); 
        } 
        _maxResponseBufferSize = value; 
    } 
}
```
Також у класі використовується автоматична властивість тільки для читання:
``` csharp
public Http2Limits Http2 { get; } = new Http2Limits();
```
Ця властивість не має окремого приватного поля та доступна тільки для читання через get.
- **Індексатори/Оператори:** Індексаторів та перевантажених операторів у класі KestrelServerLimits немає.
## 3. Практики валідації у властивостях
- **Приклад 1:** Властивість MaxResponseBufferSize має get і set та зберігає значення у приватному полі _maxResponseBufferSize. У set виконується перевірка за допомогою if: якщо значення задане і воно менше нуля, викидається виняток ArgumentOutOfRangeException. Якщо значення є коректним, воно зберігається у приватному полі _maxResponseBufferSize. Таким чином, властивість не дозволяє встановити від'ємне значення, що є прикладом валідації даних у set-аксесорі.
```csharp
public long? MaxResponseBufferSize 
{ 
    get => _maxResponseBufferSize; 
    set 
    { 
        if (value.HasValue && value.Value < 0) 
        { 
            throw new ArgumentOutOfRangeException(nameof(value), CoreStrings.NonNegativeNumberOrNullRequired); 
        } 
        _maxResponseBufferSize = value; 
    }
}

```
- **Приклад 2:** Властивість MaxRequestHeaderCount також має get і set та зберігає значення у приватному полі _maxRequestHeaderCount. У set виконується перевірка за допомогою if: якщо значення менше або дорівнює нулю, викидається виняток ArgumentOutOfRangeException. Якщо значення є коректним, воно зберігається у приватному полі _maxRequestHeaderCount. Таким чином, властивість не дозволяє встановити нульове або від'ємне значення.
```csharp
public int MaxRequestHeaderCount 
{
    get => _maxRequestHeaderCount; 
    set 
    { 
        if (value <= 0) 
        { 
            throw new ArgumentOutOfRangeException(nameof(value), CoreStrings.PositiveNumberRequired); 
        } 
        _maxRequestHeaderCount = value; 
    } 
}
```
- **Висновки щодо валідації:** У розглянутих властивостях перевірка даних виконується безпосередньо в set-аксесорі перед збереженням значення у приватному полі. Для неправильних значень використовується ArgumentOutOfRangeException. Перевагою такого підходу є те, що некоректні дані не потрапляють у внутрішній стан об'єкта. Недоліком можна вважати те, що при неправильному значенні виникає виняток, який потрібно правильно обробляти в коді.

## 4. Загальні висновки 
Під час виконання роботи було проаналізовано інкапсуляцію та валідацію даних у open-source проєкті ASP.NET Core. На прикладі класів HttpContext та HttpContextDebugView було розглянуто використання модифікаторів доступу public і private, властивостей get та set, а також readonly і sealed. Це дозволяє обмежувати прямий доступ до внутрішніх даних класів і контролювати роботу з ними.
Також було розглянуто валідацію властивостей MaxResponseBufferSize та MaxRequestHeaderCount. Перевірка значень виконується безпосередньо в set-аксесорі перед збереженням даних. Некоректні значення не приймаються, а при їх встановленні виникає виняток. Отже, інкапсуляція та валідація допомагають захищати внутрішній стан об'єктів і не допускати збереження неправильних даних.