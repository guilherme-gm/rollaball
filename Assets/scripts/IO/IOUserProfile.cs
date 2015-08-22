﻿using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;

/// <summary>
/// Representa os dados de um perfil
/// </summary>
public class UserProfile
{
	public string Name { get; set; }
	public int Points { get; set; }
	public int[] LevelInfo { get; set; }

	public UserProfile()
	{
		this.LevelInfo = new int[(int)Constants.Levels.Max];
		this.Points = 0;
	}
}

/// <summary>
/// Classe para gerenciamento
/// de perfis.
/// </summary>
public static class IOUserProfile
{
	public static string LoadedProfile = "";
	public const int FileVersion = 1;


	/// <summary>
	/// Inicializa o sistema, verificando
	/// e criando arquivos inexistentes.
	/// Deve ser executado na primeira cena do jogo.
	/// </summary>
	public static void Init()
	{
		if (!Directory.Exists("Data/"))
		    Directory.CreateDirectory("Data/");

		if (!File.Exists ("Data/roll-a-ball.dat")) {
			File.WriteAllText("Data/roll-a-ball.dat", "");
		}

		string lastProf = File.ReadAllText ("Data/roll-a-ball.dat");
		if (lastProf == "" || !File.Exists("Data/"+lastProf))
			return;

		LoadedProfile = lastProf;
	}

	/// <summary>
	/// Recebe a lista de perfis existentes
	/// </summary>
	/// <returns>Os perfis.</returns>
	public static string[] GetProfiles()
	{
		string[] profiles = Directory.GetFiles ("Data/", "userprof-*", SearchOption.TopDirectoryOnly);

		return profiles;
	}

	/// <summary>
	/// Carrega um perfil
	/// </summary>
	/// <returns>O perfil.</returns>
	/// <param name="dir">diretorio od perfil</param>
	public static UserProfile LoadProfile(string dir)
	{
		if (!File.Exists (dir))
			return null;

		UserProfile prof = new UserProfile ();
		using(BinaryReader br = new BinaryReader(File.OpenRead(dir)))
		{
			prof.Name = StringTool.GetString(br.ReadBytes(20));
			prof.Points = br.ReadInt32();

			for (int i = 0; i < (int)Constants.Levels.Max; i++) {
				prof.LevelInfo[i] = br.ReadInt32();
			}
		}

		return prof;
	}

	/// <summary>
	/// Salva o perfil
	/// </summary>
	/// <param name="profile">Perfil</param>
	public static void SaveProfile(UserProfile profile)
	{
		using(BinaryWriter bw = new BinaryWriter(File.Create("Data/userprof-"+profile.Name+".dat")))
		{
			string name = StringTool.Truncate(profile.Name, 20);
			bw.Write(Encoding.ASCII.GetBytes(name));
			bw.Write(new byte[20 - name.Length]);
			bw.Write(profile.Points);

			for (int i = 0; i < (int)Constants.Levels.Max; i++)
			{
				bw.Write (profile.LevelInfo[i]);
			}
		}
	}
}