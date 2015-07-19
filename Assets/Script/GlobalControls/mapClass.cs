using UnityEngine;
using System.Collections;
using System;



public class mapClass : IComparable<mapClass> {

	public string objName;
	public int index;
	public Vector3 objLoc;
	//public Vector3 objRot;

	public mapClass ( string objPrefab, int newIndex, Vector3 newObjectLoc) {

		objName = objPrefab;
		index = newIndex;
		objLoc = newObjectLoc;
		//objRot = newObjRot;
		}

	public int CompareTo (mapClass other){
		if (other == null) {
			return 1;
				}

		return index;
	}



}
