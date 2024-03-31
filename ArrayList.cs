using System.Collections;
class ArrayList<T> : IEnumerable where T : IComparable
{
    const int DEFAULT_SIZE = 10;

    private T[] _array;
    private int _size;
    private int _index = 0;
    public int Length
    {
        get { return _index; }
    }

    public T this[int index]
    {
        get { return GetElement(index); }
        set { SetElement(index, value); }
    }


    /*
     * 
     * Construtores
     * 
    */
    //cria um array de tamanho padrao
    public ArrayList(int size)
    {
        _array = new T[size];
        _size = size;
    }

    //cria um array com o tamanho desejado
    public ArrayList() : this( DEFAULT_SIZE )
    {

    }

    //cria um array a partir de outro
    public ArrayList(T[] array)
    {
        _array = new T[array.Length];
        _size = _array.Length;
        _index = _array.Length;

        Array.ConstrainedCopy(array, 0, _array, 0, _index);
    }

    public ArrayList(ArrayList<T> arrayList)
    {
        _array = new T[arrayList.GetArray().Length];
        _size = _array.Length;
        _index = _array.Length;

        Array.ConstrainedCopy(arrayList.GetArray(), 0, _array, 0, _index);
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
                    nameof(SetElement), "Index fora do alcance");

        _array[index] = element;
    }



    // adiciona um elemento no topo
    public void Push(T element)
    {
        if (_index < _size)
        {
            _array[_index++] = element;
            return;
        }

        //copia o array para um novo, temporario
        T[] tempArray = GetArray();

        //aumenta o tamanho do array
        _size += DEFAULT_SIZE * 2;
        _array = new T[_size];

        //copia de volta os elementos do array temporario
        Array.ConstrainedCopy(tempArray, 0, _array, 0, _index);

        //adiciona o elemento no array
        _array[_index++] = element;
    }



    // remove o ultimo elemento
    public void Pop()
    {
        if (_index - 1 < 0)
            throw new IndexOutOfRangeException("Não há elementos para serem removidos");
        
        _array[--_index] = default;

        Shrink();
    }

    // remove um elemento pelo seu index
    public void Remove(int index)
    {
        if (index >= _index || index < 0)
            throw new ArgumentOutOfRangeException(
                    nameof(Remove), "Index fora do alcance");

        Array.ConstrainedCopy(_array, index + 1, _array, index, (_index - 1) - index);
        _array[--_index] = default;

        Shrink();
    }



    // remove tamanho não utilizado
    private void Shrink()
    {
        if (_size - _index > DEFAULT_SIZE * 3)
        {
            //copia o array para um novo, temporario
            T[] tempArray = new T[_index];
            Array.ConstrainedCopy(_array, 0, tempArray, 0, _index);

            //diminui o tamanho do array
            _size -= 10;
            _array = new T[_size];

            //copia de volta os elementos do array temporario
            Array.ConstrainedCopy(tempArray, 0, _array, 0, _index);
        }
    }



    // retorna a posicao de um elemento e -1 caso nao encontre
    public int FindElement(T element)
    {
        int l = 0, r = _index - 1;
        while (l <= r)
        {
            int m = l + (r - l) / 2;

            // Check if x is present at mid
            if (element.CompareTo(_array[m]) == 0)
                return m;

            // If x greater, ignore left half
            if (element.CompareTo(_array[m]) > 0)
                l = m + 1;

            // If x is smaller, ignore right half
            else
                r = m - 1;
        }

        // If we reach here, then element was
        // not present
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
    public ArrayList<E> Map<E>(Func<T, E> func) where E : IComparable
    {
        ArrayList<E> array = new ArrayList<E>();
        for (var i = 0; i < _index; i++)
        {
            array.Push(func(_array[i]));
        }
        return array;
    }


    // loop com retorno e acumulador
    public ArrayList<E> Map<E>(Func<T, int, E> func) where E : IComparable
    {
        ArrayList<E> array = new ArrayList<E>();
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
        return (IEnumerator<T>)GetEnumerator();
    }

    public ArrayListEnum<T> GetEnumerator()
    {
        return new ArrayListEnum<T>(GetArray());
    }



    /*
     *
     *          Função Sort
     * 
     */

    public void Sort()
    {
        QuickSort(_array, 0, _index - 1);
    }

    private void QuickSort(T[] arr, int start, int end)
    {
        if (start < end)
        {
            int index_pivo = Paticionate(arr, start, end);
            QuickSort(arr, start, index_pivo - 1);
            QuickSort(arr, index_pivo + 1, end);
        }
    }

    private int Paticionate(T[] arr, int start, int end)
    {
        T pivo = arr[start];
        int i = start;

        for (int j = start + 1; j <= end; j++)
        {
            if (arr[j].CompareTo(pivo) <= 0)
            {
                i += 1;
                ChangeVariablesPosition(arr, i, j);
            }
        }

        ChangeVariablesPosition(arr, start, i);
        return i;
    }

    private void ChangeVariablesPosition(T[] arr, int a, int b)
    {
        T temp = arr[b];
        arr[b] = arr[a];
        arr[a] = temp;
    }



    // filtro
    public ArrayList<T> Filter(Func<T, bool> func)
    {
        ArrayList<T> array = new ArrayList<T>();
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
        Array.ConstrainedCopy(_array, 0, tempArray, 0, _index);
        return tempArray;
    }



    // transforma o array em string
    public string GetSring()
    {
        string str = "[ ";
        for (var i = 0; i < _index; i++)
        {
            str += _array[i];
            if (i < _index - 1) str += ",";
            str += " ";
        }
        str += "]";
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

    public ArrayListEnum(T[] array)
    {
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