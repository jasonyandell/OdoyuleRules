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
namespace OdoyuleRules
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using Configuration.RulesEngineConfigurators;
    using Models.RuntimeModel;
    using Util;

    public static class RuntimeConfiguratorExtensions
    {
        public static PropertyNode<T, TProperty> Property<T, TProperty>(
            this RuntimeConfigurator configurator, Expression<Func<T, TProperty>> propertyExpression)
            where T : class
        {
            PropertyInfo propertyInfo = propertyExpression.GetPropertyInfo();

            var fastProperty = new FastProperty<T, TProperty>(propertyInfo);

            PropertyNode<T, TProperty> propertyNode =
                configurator.CreateNode(id => new PropertyNode<T, TProperty>(id, (x, next) => next(fastProperty.Get(x))));

            return propertyNode;
        }

        public static ConditionNode<T> Condition<T>(
            this RuntimeConfigurator configurator, Predicate<T> condition)
            where T : class
        {
            ConditionNode<T> conditionNode = configurator.CreateNode(
                id => new ConditionNode<T>(id, (value, next) =>
                    {
                        if (condition(value))
                            next();
                    }));

            return conditionNode;
        }

        public static ConditionNode<Token<T1, T2>> Condition<T1, T2>(
            this RuntimeConfigurator configurator, Predicate<T2> condition)
            where T1 : class
        {
            ConditionNode<Token<T1, T2>> conditionNode = configurator.CreateNode(
                id => new ConditionNode<Token<T1, T2>>(id, (value, next) =>
                    {
                        if (condition(value.Item2))
                            next();
                    }));

            return conditionNode;
        }

        public static AlphaNode<Token<T1, T2>> Alpha<T1, T2>(
            this RuntimeConfigurator configurator)
            where T1 : class
        {
            return configurator.CreateNode(id => new AlphaNode<Token<T1, T2>>(id));
        }

        public static LeftNode<T1, T2> Left<T1, T2>(
            this RuntimeConfigurator configurator, RightActivation<T1> rightActivation)
            where T1 : class
        {
            return configurator.CreateNode(id => new LeftNode<T1, T2>(id, rightActivation));
        }

        public static JoinNode<T> Join<T>(this RuntimeConfigurator configurator,
                                          RightActivation<T> rightActivation)
            where T : class
        {
            return configurator.CreateNode(id => new JoinNode<T>(id, rightActivation));
        }


        public static DelegateProductionNode<T> Delegate<T>(this RuntimeConfigurator configurator,
                                                            Action<T> callback)
            where T : class
        {
            return configurator.CreateNode(() => new DelegateProductionNode<T>(callback));
        }
    }
}