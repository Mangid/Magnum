// Copyright 2007-2008 The Apache Software Foundation.
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
namespace Magnum.RulesEngine.ExecutionModel
{
	public class AlphaNode<T> :
		Node,
		Activation<T>,
		RightActivation<T>,
		ModelVisitorSite
	{
		private readonly SuccessorSet<T> _successors;

		public AlphaNode()
		{
			_successors = new SuccessorSet<T>();
		}

		public void Activate(RuleContext<T> context)
		{
			BetaMemory<T> betaMemory = context.GetBetaMemory(GetHashCode(), () => new BetaMemory<T>(_successors));

			betaMemory.Activate(context);
		}

		public bool Visit(ModelVisitor visitor)
		{
			return visitor.Visit(this, () => _successors.Visit(visitor));
		}

		public bool RightActivate(RuleContext<T> context)
		{
			RightActivation<T> betaMemory = context.GetBetaMemory(GetHashCode(), () => new BetaMemory<T>(_successors));

			return betaMemory.RightActivate(context);
		}

		public void AddSuccessor(params Activation<T>[] successors)
		{
			successors.Each(x => _successors.Add(x));
		}

		public MemoryJunction<T> GetConstantJunction()
		{
			return _successors
				.Get(x => x.RightActivation.GetType() == typeof (ConstantNode<T>), () => new MemoryJunction<T>(new ConstantNode<T>()));
		}

		public MemoryJunction<T> GetAlphaNodeJunction(AlphaNode<T> node)
		{
			return _successors
				.Get(x => ReferenceEquals(x.RightActivation, node), () => new MemoryJunction<T>(node));
		}
	}
}