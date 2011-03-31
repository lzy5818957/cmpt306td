using System;
using GameStateManagement;

namespace PhysicsDefense
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
            using (PhysicsDefense game = new PhysicsDefense())
            {
				game.Run();
			}
		}
	}
}

