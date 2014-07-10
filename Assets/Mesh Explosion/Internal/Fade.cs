using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Fade : MonoBehaviour {

	public Material[] materials;
	public float waitTime = 0;
	public float fadeTime = 4;
	public bool replaceShaders = true;
	
	static Dictionary<Shader, Shader> replacementShaders = new Dictionary<Shader, Shader>();
	
	public static Shader GetReplacementFor(Shader original) {
		Shader replacement;
		if (replacementShaders.TryGetValue(original, out replacement)) return replacement;
		
		const string transparentPrefix = "Transparent/";
		const string mobilePrefix = "Mobile/";
		
		var name = original.name;
		if (name.StartsWith(mobilePrefix)) {
			name = name.Substring(mobilePrefix.Length);
		}
		if (!name.StartsWith(transparentPrefix)) {
			replacement = Shader.Find(transparentPrefix + name);
		}
		
		replacementShaders[original] = replacement;
		return replacement;
	}
	
	IEnumerator Start() {
		var m = materials;
		if (m == null || m.Length == 0) materials = m = renderer.materials;
		
		if (waitTime > 0) yield return new WaitForSeconds(waitTime);
		
		SendMessage("FadeCompleted", SendMessageOptions.DontRequireReceiver);
	}

}
