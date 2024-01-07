using System.Collections;
class ArrayList<T> : IEnumerable
{
    private T[] _array;
    private int _tamanho;
    private int _index = 0;
    public int Length
    {
        get { return _index; }
    }

    public T this[int index]
    {
        get{ return GetElement(index); }
        set { SetElement(index, value); }
    }


    /*
     * 
     * Construtores
     * 
    */
    //cria um array de tamanho padrao
    public ArrayList()
    {
        _array = new T[10];
        _tamanho = 10;
    }

    //cria um array com o tamanho desejado
    public ArrayList(int tamanho)
    {
        _array = new T[tamanho];
        _tamanho = tamanho;
    }

    //cria um array a partir de outro
    public ArrayList(T[] array)
    {
        _array = new T[array.Length];
        _tamanho = _array.Length;
        _index = _array.Length;

        CopyArray(array, 0, _array, 0, _index);
    }

    public ArrayList(ArrayList<T> arrayList)
    {
        _array = new T[arrayList.GetArray().Length];
        _tamanho = _array.Length;
        _index = _array.Length;

        CopyArray(arrayList.GetArray(), 0, _array, 0, _index);
    }



    // retorna o elemento dado o index
    private T GetElement(int index)
    {
        if (index >= _index || index < 0)
            throw new ArgumentOutOfRangeException(
                    nameof(GetElement), "Index fora do alcance");

        return _array[index];
    }



    //adiciona um elemento no index definido
    private void SetElement(int index, T element)
    {
        if (index >= _index || index < 0)
            throw new ArgumentOutOfRangeException(
                    nameof(GetElement), "Index fora do alcance");

        _array[index] = element;
    }



    // adiciona um elemento no topo
    public void Push(T element)
    {
        if (_index < _tamanho)
        {
            _array[_index++] = element;
            return;
        }

        //copia o array para um novo, temporario
        T[] tempArray = GetArray();

        //aumenta o tamanho do array
        _tamanho += 10;
        _array = new T[_tamanho];

        //copia de volta os elementos do array temporario
        CopyArray(tempArray, 0, _array, 0, _index);

        //adiciona o elemento no array
        _array[_index++] = element;
    }



    // adiciona um elemento no inicio
    public void Add(T element)
    {
        if (_index < _tamanho)
        {
            CopyArray(_array, 0, _array, 1, ++_index);
            _array[0] = element;
            return;
        }

        //copia o array para um novo, temporario
        T[] tempArray = GetArray();

        //aumenta o tamanho do array
        _tamanho += 10;
        _array = new T[_tamanho];

        //copia de volta os elementos do array temporario
        CopyArray(tempArray, 0, _array, 0, _index);

        //adiciona o elemento no array
        CopyArray(_array, 0, _array, 1, ++_index);
        _array[0] = element;
    }



    // remove o ultimo elemento
    public void Pop()
    {
        if(_index <= 0)
        {
            Console.WriteLine("Não há mais elementos");
            _array[_index] = default;
            return;
        }
        _array[--_index] = default;

        Shrink();
    }



    // remove o primeiro elemento
    public void Take()
    {
        if(_index <= 0)
        {
            Console.WriteLine("Não há mais elementos");
            _array[_index] = default;
            return;
        }
        CopyArray(_array, 1, _array, 0, --_index);
        _array[_index] = default;

        Shrink();
    }



    public void Remove(int index)
    {
        if (index >= _index || index < 0)
            throw new ArgumentOutOfRangeException(
                    nameof(Remove), "Index fora do alcance");

        CopyArray(_array, index + 1, _array, index, (_index - 1) - index);
        _array[--_index] = default;

        Shrink();
    }



    // remove tamanho não utilizado
    private void Shrink()
    {
        if (_tamanho - _index > 15)
        {
            //copia o array para um novo, temporario
            T[] tempArray = new T[_index];
            CopyArray(_array, 0, tempArray, 0, _index);

            //diminui o tamanho do array
            _tamanho -= 10;
            _array = new T[_tamanho];

            //copia de volta os elementos do array temporario
            CopyArray(tempArray, 0, _array, 0, _index);
        }
    }



    // retorna a posicao de um elemento e -1 caso nao encontre
    public int FindElement(T element)
    {
        for (var i = 0; i < _index; i++)
            if (EqualityComparer<T>.Default.Equals(element, _array[i]))
                return i;
        return -1;
    }



    /*
     * 
     * Funcções de loop
     * 
    */

    // loop
    public void Map(Action<T> func)
    {
        for (var i = 0; i < _index; i++)
        {
            func(_array[i]);
        }
    }


    // loop com acumulador
    public void Map(Action<T, int> func)
    {
        for (var i = 0; i < _index; i++)
        {
            func(_array[i], i);
        }
    }


    // loop com retorno
    public ArrayList<E> Map<E>(Func<T, E> func)
    {
        ArrayList<E> array = new();
        for (var i = 0; i < _index; i++)
        {
            array.Push(func(_array[i]));
        }
        return array;
    }


    // loop com retorno e acumulador
    public ArrayList<E> Map<E>(Func<T, int, E> func)
    {
        ArrayList<E> array = new();
        for (var i = 0; i < _index; i++)
        {
            array.Push(func(_array[i], i));
        }
        return array;
    }


    // loop que altera diretamente o array
    public void ForEach(Func<T, T> func)
    {
        for (var i = 0; i < _index; i++)
        {
            _array[i] = func(_array[i]);
        }
    }


    // loop com break, caso a funcao retorne true
    public void Some(Func<T, bool> func)
    {
        for (var i = 0; i < _index; i++)
        {
            if (func(_array[i]))
                break;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
       return (IEnumerator<T>) GetEnumerator();
    }

    public ArrayListEnum<T> GetEnumerator()
    {
        return new ArrayListEnum<T>(GetArray());
    }



    // filtro
    public ArrayList<T> Filter(Func<T, bool> func)
    {
        ArrayList<T> array = new();
        for (var i = 0; i < _index; i++)
        {
            if (func(_array[i]))
                array.Push(_array[i]);
        }
        return array;
    }



    // retorna um array baseado no _array 
    public T[] GetArray()
    {
        T[] tempArray = new T[_index];
        CopyArray(_array, 0, tempArray, 0, _index);
        return tempArray;
    }



    // cópia de array
    public static void CopyArray(T[] oldArr, int initialIndex1, T[] newArr, int initialIndex2, int length)
    {
        Array.ConstrainedCopy(oldArr, initialIndex1, newArr, initialIndex2, length);
    }



    // transforma o array em string
    public string GetSring()
    {
        if(_index == 0) return "Array vazio";

        string str = "";
        for (var i = 0; i < _index; i++)
        {
            if (i == 0) str += "[ ";
            if (i == _index - 1) str += _array[i] + " ]";
            else str += _array[i] + ", ";
        }
        return str;
    }

    public void Print()
    {
        Console.WriteLine(GetSring());
    }
}





public class ArrayListEnum<T> : IEnumerator
{
    public T[] array;
    int position = -1;

    public ArrayListEnum(T[] array) {
        this.array = array;
    }

    public bool MoveNext()
    {
        position++;
        return (position < array.Length);
    }

    public void Reset()
    {
        position = -1;
    }

    object IEnumerator.Current
    {
        get
        {
            return Current;
        }
    }

    public T Current
    {
        get
        {
            try
            {
                return array[position];
            }
            catch (IndexOutOfRangeException)
            {
                throw new InvalidOperationException();
            }
        }
    }
}