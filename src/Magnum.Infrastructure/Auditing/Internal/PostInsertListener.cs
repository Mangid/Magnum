﻿// Copyright 2007-2010 The Apache Software Foundation.
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
namespace Magnum.Infrastructure.Auditing.Internal
{
	using System;
	using System.Collections.Generic;
	using Magnum.Channels;
	using NHibernate.Event;


	public class PostInsertListener :
		EventListener<PostInsertEvent>,
		IPostInsertEventListener
	{
		public PostInsertListener(UntypedChannel channel, HashSet<Type> types)
			: base(channel, types)
		{
		}

		public void OnPostInsert(PostInsertEvent @event)
		{
			OnEvent(@event);
		}

		protected override Type GetDispatchKey(PostInsertEvent e)
		{
			return e.Entity.GetType();
		}

		protected override void SendEvent<T>(PostInsertEvent @event)
		{
			var message = new PostInsertEventImpl<T>(SystemUtil.UtcNow, GetUser());

			Send<PostInsertEvent<T>>(message);
		}
	}
}