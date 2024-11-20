using System;

namespace RsTransferPort;

public class SampleLazy<T> where T : class {
    private readonly Func<T> m_valueFactory;

    // private bool isInit;
    private T m_value;

    public SampleLazy(Func<T> valueFactory, bool preLoadSceneClear = false) {
        if (valueFactory == null) throw new ArgumentNullException(nameof(valueFactory));

        m_valueFactory = valueFactory;

        if (preLoadSceneClear) App.OnPreLoadScene += ClearValue;
    }

    public T Value {
        get {
            // if (!isInit)
            // {
            //     isInit = true;
            //     m_value = m_valueFactory();
            // }

            if (m_value == null) m_value = m_valueFactory();

            return m_value;
        }
    }

    public void ClearValue() {
        m_value = default(T);

        // isInit = false;
    }
}