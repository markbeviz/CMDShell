/*
 * Created by SharpDevelop.
 * User: Beviz Mark
 * Date: 2018. 10. 03.
 * Time: 20:59
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Microsoft.Win32;

namespace AddCMDShell
{
	class Program
	{
		static string pick;
		const string DIR_LOCATION = "Directory\\shell";
		const string BACKGROUND_LOCATION  = "Directory\\Background\\shell";
		const string KEY1_TO_CREATE = "terminal";
		const string KEY2_TO_CREATE = "runas";
		const string COMMAND = "command";
		const string OPEN_CMD = "Open in CMD";
		const string OPEN_CMD_ADMIN = "Open in CMD (Administrator)";
		const string CMD_COMMAND = "cmd.exe /s /k pushd \"%V\"";

		static RegistryKey sub_key;
		
		public static void Main(string[] args)
		{
			Console.WriteLine("Get back CMD in the shell!\n\n");
			do
			{
				Console.Write("Install (i) or Uninstall (u)? ");
				pick = Console.ReadLine();
				
			} while(!pick.Equals("i") && !pick.Equals("u"));
			
			if (pick.Equals("i"))
			    Install();
			else
				Uninstall();
			Console.ReadKey();
				
		}
		
		public static void Install()
		{
			Confirm("install");
			Console.WriteLine("\nInstalling..\n");
			try
			{
				Install_Process(Registry.ClassesRoot.OpenSubKey(DIR_LOCATION, true));
				Install_Process(Registry.ClassesRoot.OpenSubKey(BACKGROUND_LOCATION, true));
				Console.Write("\nInstallation completed!\n\nPress any key to exit..");
			} 
			catch (UnauthorizedAccessException) 
			{ 
				Console.WriteLine("Could not edit Registry. Please run as Administrator.\n\nPress any key to exit..");
				Console.ReadKey();
				Environment.Exit(0);
			}
			

		}
		public static void Uninstall()
		{
			Confirm("uninstall");
			Console.WriteLine("\nUninstalling..\n");
			try
			{
				Uninstall_Process(Registry.ClassesRoot.OpenSubKey(DIR_LOCATION, true));
				Uninstall_Process(Registry.ClassesRoot.OpenSubKey(BACKGROUND_LOCATION, true));
				Console.Write("\nUninstall completed!\n\nPress any key to exit..");
			} 
			catch (Exception) 
			{ 
				Console.WriteLine("Uninstall failed. Check if the script is installed, and you have administrator rights.\n\nPress any key to exit..");
				Console.ReadKey();
				Environment.Exit(0);
			}
			

		}
		
		public static void Confirm(string what)
		{
			string input = "";
			Console.Write("Do you really want to {0} menus? (y/n) ", what);
			input = Console.ReadLine();
			if (input.Equals("y"))
				return;
			else
			{
				Console.WriteLine("Aborted!\n\nPress any key to exit..");
				Console.ReadKey();
				Environment.Exit(0);
			}
		}
		
		public static void Install_Process(RegistryKey key)
		{
			sub_key = key.CreateSubKey(KEY1_TO_CREATE);
			Console.WriteLine("[+] Added {0}", sub_key.ToString());
			sub_key.SetValue("", OPEN_CMD);
			sub_key.SetValue("Icon", "cmd.exe");
			
			sub_key = sub_key.CreateSubKey(COMMAND);
			Console.WriteLine("[+] Added {0}", sub_key.ToString());
			sub_key.SetValue("", CMD_COMMAND);
			
			
			sub_key = key.CreateSubKey(KEY2_TO_CREATE);
			Console.WriteLine("[+] Added {0}", sub_key.ToString());
			sub_key.SetValue("", OPEN_CMD_ADMIN);
			sub_key.SetValue("Icon", "cmd.exe");
			
			sub_key = sub_key.CreateSubKey(COMMAND);
			Console.WriteLine("[+] Added {0}", sub_key.ToString());
			sub_key.SetValue("", CMD_COMMAND);
			
			key.Close();
			sub_key.Close();
		}
		
		public static void Uninstall_Process(RegistryKey key)
		{
		    key.DeleteSubKey(KEY1_TO_CREATE + "\\" + COMMAND);
			Console.WriteLine("[-] Removed {0}", key.ToString() + "\\" + KEY1_TO_CREATE + "\\" + COMMAND);
			key.DeleteSubKey(KEY1_TO_CREATE);
			Console.WriteLine("[-] Removed {0}", key.ToString() + "\\" + KEY1_TO_CREATE);
						
			key.DeleteSubKey(KEY2_TO_CREATE + "\\" + COMMAND);
			Console.WriteLine("[-] Removed {0}", key.ToString() + "\\" + KEY2_TO_CREATE + "\\" + COMMAND);
			key.DeleteSubKey(KEY2_TO_CREATE);
			Console.WriteLine("[-] Removed {0}", key.ToString() + "\\" + KEY2_TO_CREATE);
			
			key.Close();
		}
	}
}