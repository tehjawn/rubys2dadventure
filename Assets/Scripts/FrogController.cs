using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FrogController : MonoBehaviour
{
  Rigidbody2D rigidbody2d;

  public float ribbitFrequency = 5.0f;
  private bool hasRibbitedRecently;
  private float ribbitTimer;

  public TextMesh frogText;

  public float displayTime = 4.0f;
  public GameObject dialogBox;
  float timerDisplay;

  private int dialogIndex = 0;
  private string[] dialogOptions = {
      "lots of broken robots...",
      "shoot 'em to fix 'em...",
      "*RIBBIT!*"
  };

  // Start is called before the first frame update
  void Start()
  {
    rigidbody2d = GetComponent<Rigidbody2D>();
    makeRibbit();

    dialogBox.SetActive(false);
    timerDisplay = -1.0f;
  }

  // Update is called once per frame
  void Update()
  {
    if (timerDisplay >= 0)
    {
      timerDisplay -= Time.deltaTime;
      if (timerDisplay < 0)
      {
        dialogBox.SetActive(false);
      }
    }

    if (hasRibbitedRecently)
    {
      ribbitTimer -= Time.deltaTime;
      if (ribbitTimer < 0)
      {
        hasRibbitedRecently = false;
        frogText.text = "";
      }
    }

    RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, new Vector2(0, -1), 1.5f, LayerMask.GetMask("Character"));
    if (hit.collider != null)
    {
      makeRibbit();
    }
  }

  void makeRibbit()
  {
    if (hasRibbitedRecently)
    {
      if (frogText != null)
      {
        frogText.text = "*ribbit*";
      }
      return;
    }

    if (frogText != null)
    {
      frogText.text = "";
    }
    hasRibbitedRecently = true;
    ribbitTimer = ribbitFrequency;
  }

  public void DisplayDialog()
  {
    Debug.Log(dialogIndex);
    timerDisplay = displayTime;
    dialogBox.SetActive(true);

    GameObject frogTextBox = GameObject.FindGameObjectWithTag("FrogDialogText");
    if (frogTextBox != null)
    {
      TextMeshProUGUI frogText = frogTextBox.GetComponent<TextMeshProUGUI>();
      if (frogText != null)
        frogText.text = dialogOptions[dialogIndex];
    }

    if (dialogIndex == 2)
      dialogIndex = 0;
    else
      dialogIndex = dialogIndex + 1;
  }
}
