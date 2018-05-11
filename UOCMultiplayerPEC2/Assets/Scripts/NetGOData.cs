using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


/*
 * Network Game Object Data, class to store position and rotation values of the gameobject.
 */
public class NetGOData {

    public static void SaveData(string name, Vector3 position, float rotation, long time) {
        string pathPos = Application.persistentDataPath + "/csv_" + name + ".csv";
        string csvAppendContent = name + "|" + position.x + "|" + position.y + "|" + position.z + "|" + rotation + "|" + time;
        try {
            if (File.Exists(pathPos)) {
                csvAppendContent = "\n" + csvAppendContent;
            } else {
                File.Create(pathPos).Close();
            }
            File.AppendAllText(pathPos, csvAppendContent);
        } catch (Exception e) {
            Debug.Log("e: " + e);
        }
    }

    public static void ClearCSVFiles() {
        DirectoryInfo info = new DirectoryInfo(Application.persistentDataPath);
        FileInfo[] fileInfo = info.GetFiles();
        foreach (FileInfo file in fileInfo) {
            string ext = file.Extension;
            if (ext.ToLowerInvariant().EndsWith("csv")) {
                try {
                    File.Delete(file.FullName);
                } catch (Exception e) {
                    Debug.Log("e: " + e);
                }
            }
        }
    }

    /*public void SaveDataCSV(string name) {
        try {
            string csvPos = "";
            string csvRot = "";
            string csvTime = "";
            for (int i = 0; i < transformPositionList.Count; i++) {
                csvPos = csvPos + "\n" + transformPositionList[i].x + "|" + transformPositionList[i].y + "|" + transformPositionList[i].z;
                csvRot = csvRot + "\n" + transformRotationList[i];
                csvTime = csvTime + "\n" + timeEpochTimeSavedData[i];
            }
            string pathPos = Application.persistentDataPath + "/csv_pos_" + name + ".csv";
            if (!File.Exists(pathPos)) {
                File.Create(pathPos).Close();
            }
            File.WriteAllText(pathPos, csvPos);

            string pathRot = Application.persistentDataPath + "/csv_rot_" + name + ".csv";
            if (!File.Exists(pathRot)) {
                File.Create(pathRot).Close();
            }
            File.WriteAllText(pathRot, csvRot);

            string pathTime = Application.persistentDataPath + "/csv_time_" + name + ".csv";
            if (!File.Exists(pathTime)) {
                File.Create(pathTime).Close();
            }
            File.WriteAllText(pathTime, csvTime);
        } catch (Exception e) {
            Debug.Log("e: " + e);
        }
    }*/

    /*public void SaveDataCSV() {
        try {
            string json = JsonUtility.ToJson(this);
            string path = Application.persistentDataPath + "/last.csv";
            if (!File.Exists(path)) {
                File.Create(path).Close();
            }
            File.WriteAllText(path, json);
        } catch (Exception e) {
            Debug.Log("e: " + e);
        }
    }

    public static RaceData LoadLastRaceData() {
        RaceData raceData = null;
        try {
            string json = File.ReadAllText(Application.persistentDataPath + "/last.json");
            raceData = JsonUtility.FromJson<RaceData>(json);
        } catch (Exception e) {
            Debug.Log("e: " + e);
        }
        return raceData;
    }*/
}

