﻿// Copyright 2011 Chris Patterson
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
namespace OdoyuleRules.Models.RuntimeModel
{
    using System;
    using System.Collections;

    public interface RuntimeModelVisitor
    {
        bool Visit(RulesEngine rulesEngine, Func<RuntimeModelVisitor, bool> next);

        // Joins
        bool Visit<T>(ConstantNode<T> node, Func<RuntimeModelVisitor, bool> next)
            where T : class;

        bool Visit<T>(JoinNode<T> node, Func<RuntimeModelVisitor, bool> next)
            where T : class;

        bool Visit<T, TDiscard>(LeftJoinNode<T, TDiscard> node, Func<RuntimeModelVisitor, bool> next)
            where T : class;

        bool Visit<TLeft, TRight>(OuterJoinNode<TLeft, TRight> node, Func<RuntimeModelVisitor, bool> next)
            where TLeft : class
            where TRight : class;

        // fact nodes
        bool Visit<T>(AlphaNode<T> node, Func<RuntimeModelVisitor, bool> next)
            where T : class;

        bool Visit<TInput, TOutput>(WidenTypeNode<TInput, TOutput> node, Func<RuntimeModelVisitor, bool> next)
            where TInput : class, TOutput
            where TOutput : class;

        // condition nodes

        bool Visit<T, TProperty>(PropertyNode<T, TProperty> node, Func<RuntimeModelVisitor, bool> next)
            where T : class;

        bool Visit<T>(ConditionNode<T> node, Func<RuntimeModelVisitor, bool> next)
            where T : class;

        bool Visit<T, TProperty>(EqualNode<T, TProperty> node, Func<RuntimeModelVisitor, bool> next)
            where T : class;

        bool Visit<T, TProperty>(ValueNode<T, TProperty> node, Func<RuntimeModelVisitor, bool> next)
            where T : class;

        bool Visit<T, TProperty>(CompareNode<T, TProperty> node, Func<RuntimeModelVisitor, bool> next)
            where T : class;

        bool Visit<T, TProperty>(NotNullNode<T, TProperty> node, Func<RuntimeModelVisitor, bool> next)
            where T : class
            where TProperty : class;

        bool Visit<T, TProperty>(ExistsNode<T, TProperty> node, Func<RuntimeModelVisitor, bool> next)
            where T : class
            where TProperty : class, IEnumerable;

        bool Visit<T, TProperty, TElement>(EachNode<T, TProperty, TElement> node, Func<RuntimeModelVisitor, bool> next)
            where T : class
            where TProperty : class, IEnumerable;

        // production nodes

        bool Visit<T>(DelegateProductionNode<T> node, Func<RuntimeModelVisitor, bool> next)
            where T : class;

        bool Visit<T, TFact>(AddFactProductionNode<T, TFact> node, Func<RuntimeModelVisitor, bool> next)
            where T : class
            where TFact : class;
    }
}