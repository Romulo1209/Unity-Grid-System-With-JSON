using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridController : MonoBehaviour
{
    [Header("Grid Creation")]
    public bool CreateGrid;
    public TextAsset json;
    public GridSetup grid;
    [Header("Others")]
    public InformationsUI informationsUI;
    public IndexBlock selected;
    public LayerMask layerMask;

    [Header("References")]
    public GameObject GridPrefab;
    public GridScriptable gridSprite;
    public Transform Father;
    public Camera _Cam;

    private void Start()
    {
        GridSetup();
    }
    private void Update()
    {
        RayCastSelect();
        if (Input.GetKeyDown(KeyCode.E))
            CreateJSON(grid);
    }

    #region GridSetup
    public async void GridSetup()
    {
        if (CreateGrid)
            await MakeGrid(grid.GridHeight, grid.GridWidth);
        else
            await GetJSON(json.ToString());

        foreach (GridLines line in grid.GridLines) {
            foreach (GridInfos info in line._GridLines) {
                await CheckOwner(info.GridPositionX, info.GridPositionY, info.GridOwner, info);
            }
        }
    }
    public async Task GetJSON(string json)
    {
        grid = JsonUtility.FromJson<GridSetup>(json);
        foreach(GridLines grid in grid.GridLines) {
            foreach(GridInfos info in grid._GridLines) {
                GameObject obj = Instantiate(GridPrefab, new Vector3(info.GridPositionX, 0, -info.GridPositionY), GridPrefab.transform.rotation, Father);
                obj.name = $"X {info.GridPositionX.ToString("000")}, Y {info.GridPositionY.ToString("000")}";
                obj.GetComponent<IndexBlock>().IndexX = info.GridPositionX;
                obj.GetComponent<IndexBlock>().IndexY = info.GridPositionY;
                info.obj = obj;
                await Task.Yield();
            }
        }
    }
    public async Task MakeGrid(int height, int width)
    {
        for (int y = 0; y < height; y++) {
            GridLines line = new GridLines();
            List<GridInfos> list = new List<GridInfos>();
            line.Line = $"Line {y}";
            for (int x = 0; x < width; x++) {
                GridInfos grid = new GridInfos();
                GameObject obj = Instantiate(GridPrefab, new Vector3(x, 0, -y), GridPrefab.transform.rotation, Father);
                obj.name = $"X {x.ToString("000")}, Y {y.ToString("000")}";
                obj.GetComponent<IndexBlock>().IndexX = x;
                obj.GetComponent<IndexBlock>().IndexY = y;

                grid.Position = $"{x} {y}";
                grid.GridOwner = $"";
                grid.GridName = $"";
                grid.GridSize = $"";
                grid.GridPositionX = x;
                grid.GridPositionY = y;
                grid.obj = obj;

                list.Add(grid);
                await Task.Yield();
            }
            line._GridLines = list;
            grid.GridLines.Add(line);
        }
    }
    public async Task CheckOwner(int posX, int posY, string owner, GridInfos info)
    {
        bool up = false, down = false, left = false, right = false;
        if(owner != "" || owner != string.Empty)
        {
            //Checar se existe bloco acima
            if (posY - 1 >= 0)
                if (grid.GridLines[posY - 1]._GridLines[posX].GridOwner == owner) {
                    info.Up = true;
                    up = true;
                }
            //Checar se existe bloco abaixo
            if (posY + 1 <= grid.GridLines.Count - 1)
                if (grid.GridLines[posY + 1]._GridLines[posX].GridOwner == owner) {
                    info.Down = true;
                    down = true;
                }
            //Checar se existe bloco a esquerda
            if (posX - 1 >= 0)
                if (grid.GridLines[posY]._GridLines[posX - 1].GridOwner == owner) {
                    info.Left = true;
                    left = true;
                }
            //Checar se existe bloco a direita
            if (posX + 1 <= grid.GridLines[posY]._GridLines.Count - 1)
                if (grid.GridLines[posY]._GridLines[posX + 1].GridOwner == owner) {
                    info.Right = true;
                    right = true;
                }
        }
        
        DelegateSprite(up, down, left, right, info);
        info.obj.GetComponent<SpriteRenderer>().sprite = info.TileSprite;
        await Task.Yield();
    }
    void DelegateSprite(bool up, bool down, bool left, bool right, GridInfos info)
    {
        if (!up && down && !left && right) {
            info.SpriteOrientation = GridInfos.Delegation.UpperLeft;
            info.TileSprite = gridSprite.UpperLeftBox;
        }
        else if (!up && down && left && right) {
            info.SpriteOrientation = GridInfos.Delegation.UpperMiddle;
            info.TileSprite = gridSprite.UpperMiddleBox;
        }
        else if (!up && down && left && !right) {
            info.SpriteOrientation = GridInfos.Delegation.UpperRight;
            info.TileSprite = gridSprite.UpperRightBox;
        }
        else if (up && down && !left && right) {
            info.SpriteOrientation = GridInfos.Delegation.MiddleLeft;
            info.TileSprite = gridSprite.MiddleLeftBox;
        }
        else if (up && down && left && right) {
            info.SpriteOrientation = GridInfos.Delegation.Middle;
            info.TileSprite = gridSprite.MiddleBox;
        }
        else if (up && down && left && !right) {
            info.SpriteOrientation = GridInfos.Delegation.MiddleRight;
            info.TileSprite = gridSprite.MiddleRightBox;
        }
        else if (up && !down && !left && right) {
            info.SpriteOrientation = GridInfos.Delegation.BottonLeft;
            info.TileSprite = gridSprite.BottonLeftBox;
        }
        else if (up && !down && left && right) {
            info.SpriteOrientation = GridInfos.Delegation.BottonMiddle;
            info.TileSprite = gridSprite.BottonMiddleBox;
        }
        else if (up && !down && left && !right)
        {
            info.SpriteOrientation = GridInfos.Delegation.BottonRight;
            info.TileSprite = gridSprite.BottonRightBox;
        }
        else {
            info.SpriteOrientation = GridInfos.Delegation.FullBox;
            info.TileSprite = gridSprite.FullBox;
        }
    }
    #endregion

    #region LandInfos
    void RayCastSelect()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 30f;
        mousePosition = _Cam.ScreenToWorldPoint(mousePosition);
        Debug.DrawRay(_Cam.transform.position, mousePosition - _Cam.transform.position, Color.blue);
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _Cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 100, layerMask))
            {
                Debug.Log(EventSystem.current.IsPointerOverGameObject());
                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                if (selected != null) {
                    selected.Selected(false);
                }
                selected = hit.transform.gameObject.GetComponent<IndexBlock>();
                selected.Selected(true);

                GridInfos info = grid.GridLines[hit.transform.gameObject.GetComponent<IndexBlock>().IndexY]._GridLines[hit.transform.gameObject.GetComponent<IndexBlock>().IndexX];
                SetInformationsWindow(info);

                AnimatorClipInfo[] clip;
                clip = informationsUI.informationsAnim.GetCurrentAnimatorClipInfo(0);
                if(clip[0].clip.name == "InfosHide") {
                    informationsUI.informationsAnim.SetTrigger("Switch");
                }
            }
        }
    }
    void SetInformationsWindow(GridInfos info)
    {
        informationsUI.LandOwner.text = info.GridOwner;
        informationsUI.LandName.text = info.GridName;
        informationsUI.LandDescription.text = info.GridDescription;
        informationsUI.LandLocation.text = $"X {info.GridPositionX} Y {info.GridPositionY}";
        informationsUI.LandSize.text = info.GridSize.ToString();
    }
    public void CloseInformations() {
        informationsUI.informationsAnim.SetTrigger("Switch");
        selected.Selected(false);
        selected = null;
    }
    #endregion

    #region Create
    void CreateJSON(GridSetup grid)
    {
        string jsonFilePath = Application.dataPath + "/gridData.json";
        string json = "";
        if (json == string.Empty)
            json = JsonUtility.ToJson(grid, true);
        else
            return;


        if (!File.Exists(jsonFilePath))
        {
            File.WriteAllText(jsonFilePath, json);
        }
    }
    #endregion
}

#region Serialization Classes
[System.Serializable]
public class InformationsUI
{
    public Image LandImage;
    public Text LandOwner;
    public Text LandName;
    public Text LandDescription;
    public Text LandLocation;
    public Text LandSize;
    public Animator informationsAnim;
}
[System.Serializable]
public class GridSetup
{
    public int GridHeight;
    public int GridWidth;
    [Space]
    public List<GridLines> GridLines;
}
[System.Serializable]
public class GridLines
{
    public string Line;
    public List<GridInfos> _GridLines;
}
[System.Serializable]
public class GridInfos
{
    public enum Delegation { UpperLeft, UpperMiddle, UpperRight, MiddleLeft, Middle, MiddleRight, BottonLeft, BottonMiddle, BottonRight, FullBox}

    [Header("Grid Infos")]
    public string Position;
    public string GridName;
    [TextArea(5,15)] public string GridDescription;
    public string GridOwner;
    public string GridSize;
    [Space]
    public int GridPositionY;
    public int GridPositionX;
    public GameObject obj;
    [Space]
    public bool Up;
    public bool Right;
    public bool Down;
    public bool Left;
    [Space]
    public Delegation SpriteOrientation;
    public Sprite TileSprite;
}
#endregion