﻿using System;
using System.Collections.Generic;

namespace Discord.Modules
{
	public class ModuleService : IService
	{
		public DiscordClient Client { get; private set; }
        
		public IEnumerable<ModuleManager> Modules => _modules.Values;
		private readonly Dictionary<IModule, ModuleManager> _modules;

		public ModuleService()
		{
			_modules = new Dictionary<IModule, ModuleManager>();
		}

		void IService.Install(DiscordClient client)
		{
            Client = client;
        }

		public T Add<T>(T module, string name, ModuleFilter type)
			where T : class, IModule
		{
			if (module == null) throw new ArgumentNullException(nameof(module));
			if (name == null) throw new ArgumentNullException(nameof(name));
			if (Client == null)
                throw new InvalidOperationException("Service needs to be added to a DiscordClient before modules can be installed.");
            if (_modules.ContainsKey(module))
                throw new InvalidOperationException("This module has already been added.");

			var manager = new ModuleManager(Client, module, name, type);
			_modules.Add(module, manager);
			module.Install(manager);
            return module;
        }

		public ModuleManager GetManager(IModule module)
		{
			if (module == null) throw new ArgumentNullException(nameof(module));

			ModuleManager result = null;
			_modules.TryGetValue(module, out result);
			return result;
		}
	}
}
