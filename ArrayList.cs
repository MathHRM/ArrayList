class ArrayList<T>
{
    private T[] _array;
    private int _tamanho;
    private int _index = 0;
    public int Length
    {
        get { return _index; }
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
    public T GetElement(int index)
    {
        if (index >= _index || index < 0)
            throw new ArgumentOutOfRangeException(
                    nameof(GetElement), "Index fora do alcance");

        return _array[index];
    }



    //adiciona um elemnto no index definido
    public void SetElement(int index, T element)
    {
        if (index >= _index || index < 0)
            throw new ArgumentOutOfRangeException(
                    nameof(GetElement), "Index fora do alcance");

        _array[index] = element;
    }



    // adiciona um elemento no ultimo index
    public void PushElement(T element)
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



    // remove o ultimo elemento
    public void RemoveElement()
    {
        _array[--_index] = default;

        Diminuir();
    }

    public void RemoveElement(int index)
    {
        if (index >= _index || index < 0)
            throw new ArgumentOutOfRangeException(
                    nameof(RemoveElement), "Index fora do alcance");

        CopyArray(_array, index+1, _array, index, (_index-1) - index);
        _array[--_index] = default;

        Diminuir();
    }

    // remove tamanho não utilizado
    private void Diminuir()
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



    // funcao de loop
    public void Map(Action<T> func)
    {
        for (var i = 0; i < _index; i++)
        {
            func(GetElement(i));
        }
    }

    public void Map(Action<T, int> func)
    {
        for (var i = 0; i < _index; i++)
        {
            func(GetElement(i), i);
        }
    }

    public ArrayList<T> Map(Func<T, T> func)
    {
        ArrayList<T> array = new();
        for (var i = 0; i < _index; i++)
        {
            var e = GetElement(i);
            array.PushElement(func(e));
        }
        return array;
    }

    public ArrayList<T> Map(Func<T, int, T> func)
    {
        ArrayList<T> array = new();
        for (var i = 0; i < _index; i++)
        {
            var e = GetElement(i);
            array.PushElement(func(e, i));
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



    // funcao de filtro
    public ArrayList<T> Filter(Func<T, bool> func)
    {
        ArrayList<T> array = new();
        for (var i = 0; i < _index; i++)
        {
            var e = GetElement(i);
            if (func(e))
                array.PushElement(e);
        }
        return array;
    }



    // funcao de loop com break, caso a funcao retorne true
    public void Some(Func<T, bool> func)
    {
        for (var i = 0; i < _index; i++)
        {
            var e = GetElement(i);
            if (func(e))
                break;
        }
    }



    // retorna um array baseado no _array 
    public T[] GetArray()
    {
        T[] tempArray = new T[_index];
        Array.ConstrainedCopy(_array, 0, tempArray, 0, _index);
        return tempArray;
    }



    // função de cópia
    public static void CopyArray(T[] oldArr, int initialIndex1, T[] newArr, int initialIndex2, int length)
    {
        Array.ConstrainedCopy(oldArr, initialIndex1, newArr, initialIndex2, length);
    }



    // transforma o array em string
    public string GetSring()
    {
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
