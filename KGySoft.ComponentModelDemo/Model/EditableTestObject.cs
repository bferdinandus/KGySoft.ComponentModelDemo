﻿using KGySoft.ComponentModel;

namespace KGySoft.ComponentModelDemo.Model
{
    public class EditableTestObject : EditableObjectBase, ITestObject
    {
        public int IntProp
        {
            get => Get<int>();
            set => Set(value);
        }

        public string StringProp
        {
            get => Get<string>();
            set => Set(value);
        }
    }
}