# NetGame-Unity

## Konwencja nazewnicza

Deklaracje klas, struktur:
```csharp
class/struct UpperCamelCase

public class/struct UpperCamelCase

class UpperCamelCase : BaseClass
```

Pola publiczne: `typ loweCamelCase`

Pola prywatne: `typ _loweCamelCase`

Funkcje i metody: `typ UpperCamelCase(typ lowerCamelCase)`

Eventy: `public static event typ OnUpperCamelCaseEvent;`

Eventy Unity: `public UnityEvent onUpperCameCase;`

Enumy:
```csharp
enum UpperCamelCase
{
    UpperCamelCase1,
    UpperCamelCase2
}
```

**Klamry na osobnych liniach.**

### Przykład
```csharp
public class SomeComponent : MonoBehaviour 
{
    public UnityEvent onSomethingHappened;
    public static event System.Action OnSomethingHappenedEvent;

    public float somePublicVariable;
    private float _somePrivateVariable;

    public enum MyEnum
    {
        One,
        Two,
        Three
    }

    public MyEnum oneTwoOrThree;

    void SomeMethod()
    {
    }

    void SomeOtherMethod(int integerParam)
    {
    }
}
```

## Praca z Unity

Aby uniknąć konfilktów (których bardzo często nie da się rozwiązać i kończą się utraceniem pracy jednej z osób), każdy powinien pracować na własnej scenie.

Scena `Main` będzie zmieniana tylko po ustaleniu tego z innymi członkami zespołu.

Używajcie jak najwięcej prefab'ów, **ALE** jeżeli cenicie swoje zdrowie psychiczne **NIE UŻYWAJCIE ZAGNIEŻDŻONYCH PREFAB'ÓW**.

Struktura w katalogu Assets wygląda następująco:
```
Assets
    - Playgrounds [1]
    - PostProcessing [2]
    - Resources [3]
        - Animations
            - Animators
        - Materials
        - Misc
        - PhysicsMaterials
        - Prefabs
        - Scripts
            - Debug
```

1. Tutaj tworzymy sceny na których pracujemy
2. Jest to katalog utworzony przez dodatek PostProcessingStack, więcej takich katalogów może pojawić się w przyszłości.
3. Wszelkie asset'y tworzone przez nas powinny pojawiać się tutaj w odpowiednich katalogach.

## Styl pisania komponentów

Komponenty powinny być tak napisane aby w jak największym stopniu wspierać reuse.

### Interfejs publiczny komponentu

Komponenty, które eksponują jakąś funkcjonalność powinny posiadać interfejs zawarty w regionie PublicInterface. Interfejs powinien składać się z metod publicznych oraz z event'ów (tych C#'owych oraz UnityEvent'ów).

Przykład:

```csharp
public class SomeComponent : MonoBehaviour 
{
    /*
        Class body...
    */

#region PublicInterface

    public static event System.Action OnSomethingHappenedEvent;
    public UnityEvent onSomethingHappened;

    public void Stop()
    {
        // Method body
    }

#endregion
}
```

### Zalecane jest używanie zagnieżdżonych, serializowalnych struct'ów, np.

```csharp
[System.Serializable]
public struct Parameters
{
    public float helath;
    public float helathRegenPerSecond;
}
```

Dzięki temu unikniemy bałaganu w inspektorze Unity.

### Komunikacja pomiędzy obiektami

**Nie używamy mechanizmu SendMessage, BroadcastMessage, etc. - ponieważ:**

1. Jest to najwolniejszy mechanizm do komunikacji pomiędzy obiektami.
2. Metody są wywoływane po nazwach (string'ach) co powoduje problemy z refactoringiem.

Do komunikacji powinniśmy używać:
1. `GetComponent<TypComponentu>()` - przy czym, jeżeli tylko jest to możliwe to:
    
    1.1. `TypComponentu` powinien być interfejsem lub klasą bazową.

    1.2. `GetComponent` powinien być wywołany raz w funkcji start a jego wynik zapisany do zmiennej prywatnej.

2. UnityEvent'ów

    UnityEvent'y to bardzo wygodny mechanizm, pozwalający na określenie komunikacji pomiędzy obiektami z poziomu inspektora.

    Najlepiej się sprawdza do określania komunikacji pomiędzy obiektami w dół, lub na tym samym poziomie, hierarchii (np. komunikacja pomiędzy różnymi komponentami i obiektami wewnątrz gracza).

    Nie jest zbyt dobrym rozwiązaniem jeżeli chodzi o komunikację pomiędzy obiektami sąsiadującymi w hierarchii.

3. Eventów i delegat:

    Aby korzystać z eventów, należy sprecyzować odpowiedni typ delegaty. Można to zrobić ręcznie, np.

    `public delegate void MyDelegate(int param);`

    Ale lepiej będzie wykorzystać gotowe generyczne delegaty C#, czyli:

    `System.Action` oraz `System.Func`

    `System.Action` zwraca zawsze void i posiada parametry generyczne, pozwalające na sprecyzowanie do 16 argumentów naszej delegaty, np.

    `System.Action<int, int>` to delegata o typie zwrotu `void` i 2 argumentach typu `int`. Funkcja, która pasuje do takiej deklaracji to np.
    `void MyMethod(int a, int b) {} `

    `System.Func` posiada zawsze przynajmniej jeden parametr generyczny określający typ zwrotu (ostatni parametr), np.

    `System.Func<int>` to delegata zwracająca `int`, nie przyjmująca żadnych argumentów.
    `System.Func<int, float, bool>` to delegate przyjmująca `int` oraz `float` i zwracająca `bool`. Funkcja, która pasuje do takiej deklaracji to np.
    `bool MyMethod(int a, float b) {}`

    **Korzystanie z eventów**

    Istotne są tak naprawdę 2 rzeczy:

    1. Przed każdym wywołaniem event'u należy sprawdzić, czy jest on różny od null'a.
    2. Subskrypcja metod do takiego event'ów powinna się odbywać w metodach OnEnable i OnDisable.

    **Przykład poprawnego wykorzystania eventów:**

    ```csharp
    // Plik ComponentA.cs
    class ComponentA : MonoBehaviour
    {
        public static event System.Action OnSomethingEvent;

        void Update()
        {
            if (OnSomethingEvent != null)
                OnSomethingEvent();
        }
    }

    // Plik ComponentB.cs
    class ComponentB : MonoBehaviour
    {
        void OnEnable()
        {
            ComponentA.OnSomethingEvent += OnSomethingEventResponder;
        }

        void OnDisable()
        {
            ComponentA.OnSomethingEvent -= OnSomethingEventResponder;
        }

        void OnSomethingEventResponder()
        {
            Debug.Log("I respond.");
        }
    }
    ```