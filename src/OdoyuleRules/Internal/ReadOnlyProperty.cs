// Copyright 2011 Chris Patterson
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace OdoyuleRules.Internal
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;


    class ReadOnlyProperty<T, TProperty>
    {
        public readonly Func<T, TProperty> GetDelegate;

        public ReadOnlyProperty(PropertyInfo property)
        {
            Property = property;
            GetDelegate = GetGetMethod(Property);
        }

        public PropertyInfo Property { get; private set; }

        public TProperty Get(T instance)
        {
            return GetDelegate(instance);
        }

        static Func<T, TProperty> GetGetMethod(PropertyInfo property)
        {
            ParameterExpression instance = Expression.Parameter(typeof (T), "instance");
            return
                Expression.Lambda<Func<T, TProperty>>(Expression.Call(instance, property.GetGetMethod()), instance).
                    Compile();
        }
    }
}