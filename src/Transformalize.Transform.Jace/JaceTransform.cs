#region license
// Transformalize
// Configurable Extract, Transform, and Load
// Copyright 2013-2017 Dale Newman
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
//       http://www.apache.org/licenses/LICENSE-2.0
//   
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Jace;
using Jace.Execution;
using Transformalize.Contracts;

namespace Transformalize.Transforms.Jace {
    public class JaceTransform : BaseTransform {

        private readonly Func<IRow, object> _transform;

        public JaceTransform(IContext context = null) : base(context, "double") {
            if (IsMissingContext()) {
                return;
            }

            Returns = Context.Field.Type;

            if (IsMissing(Context.Operation.Expression)) {
                return;
            }
            var input = Context.Entity.GetFieldMatches(Context.Operation.Expression).ToArray();
            var engine = new CalculationEngine(CultureInfo.CurrentCulture, ExecutionMode.Compiled);

            try {

                var variables = new Dictionary<string, double>();

                foreach (var field in input) {
                    variables.Add(field.Alias, 1.0);
                }

                var defaultResult = engine.Calculate(Context.Operation.Expression, variables);
                Context.Debug(() => $"Jace compiled {Context.Operation.Expression} with all ones and returned {defaultResult}.");

                _transform = row => Context.Field.Convert(engine.Calculate(Context.Operation.Expression, input.ToDictionary(k => k.Alias, v => v.Type == "double" ? (double)row[v] : Convert.ToDouble(row[v]))));

            } catch (ParseException ex) {
                Context.Error($"The expression {Context.Operation.Expression} in field {Context.Field.Alias} can not be parsed. {ex.Message}");
                Run = false;
            }

        }

        public override IRow Operate(IRow row) {
            row[Context.Field] = _transform(row);
            return row;
        }

        public override IEnumerable<OperationSignature> GetSignatures() {
            return new[]{
                new OperationSignature("jace") {
                    Parameters = new List<OperationParameter> {new OperationParameter("expression")}
                }
            };
        }
    }
}
