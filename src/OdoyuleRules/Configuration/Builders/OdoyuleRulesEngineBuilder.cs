// Copyright 2011-2012 Chris Patterson
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
namespace OdoyuleRules.Configuration.Builders
{
    using System;
    using Compilation;
    using Internal.Caching;
    using RuntimeModelConfigurators;


    public class OdoyuleRulesEngineBuilder :
        RulesEngineBuilder
    {
        readonly Cache<string, Rule> _rules;
        Func<RuntimeConfigurator> _runtimeConfiguratorFactory;

        public OdoyuleRulesEngineBuilder()
        {
            _runtimeConfiguratorFactory = DefaultRuntimeConfiguratorFactory;

            _rules = new DictionaryCache<string, Rule>(rule => rule.RuleName);
        }

        public void AddRule(Rule rule)
        {
            _rules.AddValue(rule);
        }

        public RulesEngine Build()
        {
            RuntimeConfigurator runtimeConfigurator = _runtimeConfiguratorFactory();

            CompileRules(runtimeConfigurator);

            return runtimeConfigurator.RulesEngine;
        }

        void CompileRules(RuntimeConfigurator runtimeConfigurator)
        {
            RuleCompiler compiler = new OdoyuleRuleCompiler(runtimeConfigurator);

            _rules.Each(compiler.Compile);
        }

        static RuntimeConfigurator DefaultRuntimeConfiguratorFactory()
        {
            return new OdoyuleRuntimeConfigurator();
        }
    }
}