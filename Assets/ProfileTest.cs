using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ProfileTest : MonoBehaviour {
	public static ProfileTest Instance;

	public Transform ProfileList;
	public GameObject ProfileEntry;


	public Text CreateName;
	public Text CreatePts;
	public Toggle CreateLevel1;
	public Toggle CreateLevel2;


	public Text DataName;
	public Text DataPts;
	public Text DataLevels;


	// Use this for initialization
	void Start () {
		Instance = this;

		// Deve ser chamado na primeira cena
		IOUserProfile.Init ();

		// Recebe a lista de perfis
		string[] profiles = IOUserProfile.GetProfiles ();

		// Adiciona os perfis em uma lista
		for (int i = 0; i < profiles.Length; i++) {
			GameObject tempProfEntry = GameObject.Instantiate(ProfileEntry) as GameObject;
			MainMenuUIProfiles profData = tempProfEntry.GetComponent<MainMenuUIProfiles>();
			profData.ProfileName.text = profiles[i];

			tempProfEntry.transform.SetParent(ProfileList);
		}
	}

	public void OnProfileLoad(string profName)
	{
		UserProfile profile = IOUserProfile.LoadProfile (profName);

		DataName.text = "Nome: " + profile.Name;
		DataPts.text = "Pontos: " + profile.Points;
		DataLevels.text = "Fases: " + (profile.LevelInfo [0] == 1 ? "Aberta" : "Fechada")  + 
			" | " + (profile.LevelInfo [1] == 1 ? "Aberta" : "Fechada");
	}

	public void OnProfileCreate()
	{
		UserProfile prof = new UserProfile ();
		prof.Name = CreateName.text;
		prof.Points = Int32.Parse(CreatePts.text);
		prof.LevelInfo = new int[(int)Constants.Levels.Max];
		prof.LevelInfo [0] = (CreateLevel1.isOn ? 1 : 0);
		prof.LevelInfo [1] = (CreateLevel2.isOn ? 1 : 0);
		IOUserProfile.SaveProfile (prof);

		// Usado para recarregar a cena e atualizar a lista
		// pode ser feito por outros metodos mais eficientes
		// deixei assim para simplicidade
		Application.LoadLevel (Application.loadedLevel);
	}
}
