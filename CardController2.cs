 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;


public class CardController2 : MonoBehaviour
{
    [SerializeField][Tooltip("monster positions on player duel mat")]
    private List<Transform> MonsterPositions;
    [SerializeField][Tooltip("spell/trap positions on player duel mat")]
    private List<Transform> SpellPositions;
    [SerializeField][Tooltip("field spell card position")]
    private Transform FieldPosition;

    [SerializeField]
    private List<Transform> CardsPositionsInHand;
    [SerializeField]
    private List<Transform> CardsInHand;
    [SerializeField]
    private List<GameObject> MonsterCardsOnField;
    
    [SerializeField]
    private GameObject cardPrefab;
    [SerializeField]
    private List<Card> CardPool;
    
    [SerializeField][Tooltip("cardsInHand Transform")]
    private Transform PlayerHand;
    [SerializeField][Tooltip("cardsOnField Transform")]
    private Transform CardsInPlay;

    [SerializeField]
    private TextMeshProUGUI LifePoints;
    [SerializeField]
    private TextMeshProUGUI oppAttkPoints;
    [SerializeField]
    private TextMeshProUGUI oppDefPoints;
    [SerializeField]
    private TextMeshProUGUI myAttkPoints;
    [SerializeField]
    private TextMeshProUGUI myDefPoints;


    [SerializeField]
    private GameObject DuelController;


    private PlayerInput playerInput;

    private InputAction confirmAction;
    private InputAction declineAction;

    private GameObject card;

    [SerializeField]
    private GameObject currentCard;


    [SerializeField]
    private float mouseDragSpeed = 0.1f;

    private Vector3 velocity = Vector3.zero;
    private Camera mainCamera;


    [SerializeField]
    private float minDistance = 0.5f;

    [SerializeField]
    private bool isAttacking = false;


    [SerializeField]
    private ParticleSystem particle;

    void Awake()
    {
        Cursor.visible = true;
        playerInput = GetComponent<PlayerInput>();

        FillHand();
        Hand();
        
        confirmAction = playerInput.actions["Confirm"];
        
        confirmAction.performed += _ => selectCard();
        confirmAction.canceled += _ => MouseReleased();
        
        mainCamera = Camera.main;
        
    }



// XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    public void Update(){
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            if(hit.collider != null && hit.collider.gameObject.CompareTag("Played"))
            {
                MonsterCard c = (MonsterCard)hit.collider.gameObject.GetComponent<CardDisplay>().card;
               
                int atk = c.Attack;
                int def = c.Defence;
                
                myAttkPoints.text = atk.ToString();
                myDefPoints.text = def.ToString();
            }
            else if(hit.collider != null && hit.collider.gameObject.CompareTag("OpponentCard"))
            {
                MonsterCard o = (MonsterCard)hit.collider.gameObject.GetComponent<CardDisplay>().card;
                int Oatk = o.Attack;
                int Odef = o.Defence;
                
                oppAttkPoints.text = Oatk.ToString();
                oppDefPoints.text = Odef.ToString();
            }   

        }
    }

    // action for mouseclicked.performed;
    private void selectCard()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            if(hit.collider != null && hit.collider.gameObject.CompareTag("Card"))
            {
                currentCard = hit.collider.gameObject;
                StartCoroutine(DragUpdate(currentCard));
            }        
            else if (hit.collider.gameObject.tag == "Played")
            {
                currentCard = hit.collider.gameObject;
            }    
        }
        for(int i=0; i<CardsPositionsInHand.Count;i++){
            if(currentCard == CardsPositionsInHand[i].GetComponent<CardPosition>().card){
                CardsPositionsInHand[i].GetComponent<CardPosition>().isOccupied = false;
            }
        }
    }


    private IEnumerator DragUpdate(GameObject clickedObject)
    {
        float initialDistance = Vector3.Distance(clickedObject.transform.position, mainCamera.transform.position);
        
        while(confirmAction.ReadValue<float>() != 0)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            Vector3 direction = ray.GetPoint(initialDistance) - clickedObject.transform.position;
            clickedObject.transform.position = Vector3.SmoothDamp(clickedObject.transform.position, ray.GetPoint(initialDistance), ref velocity, mouseDragSpeed);
            yield return null;
        }
    }

    // end of action for mouseclicked.performed;


    //action for mousereleased
    private void MouseReleased()
    {
        // if current card is monster
        if(currentCard.GetComponent<CardDisplay>().card.category == Card.Category.Normal && currentCard.tag != "Played")
        {
            for(int i=0; i<MonsterPositions.Count; i++)
            {
                float test = Vector3.Distance(MonsterPositions[i].transform.position, currentCard.transform.position);
                test = Mathf.Abs(test);
                test = Mathf.Floor(test*100);
                test = test * test;
                if(test < minDistance && MonsterPositions[i].GetComponent<CardPosition>().isOccupied == false)
                {
                    currentCard.transform.position = MonsterPositions[i].transform.position;
                    currentCard.transform.rotation = MonsterPositions[i].transform.rotation;
                    MonsterPositions[i].GetComponent<CardPosition>().isOccupied = true;
                    MonsterPositions[i].GetComponent<CardPosition>().card = currentCard;
                    currentCard.tag = "Played";
                    MonsterCardsOnField.Add(currentCard);
                    DuelController.GetComponent<DuelController>().SummonMonster(currentCard.GetComponent<CardDisplay>().card.CardName, i);
                    currentCard = null;
                    return;
                }
                else
                {
                    for(int j=0; j<CardsPositionsInHand.Count; j++)
                    {
                        if(CardsPositionsInHand[j].GetComponent<CardPosition>().isOccupied == false)
                        {
                            currentCard.transform.position = CardsPositionsInHand[j].transform.position;
                            currentCard.transform.rotation = CardsPositionsInHand[j].transform.rotation;
                            CardsPositionsInHand[j].GetComponent<CardPosition>().isOccupied = true;
                        }
                    }
                }
            }
        }


        // if current card is field spell
        if(currentCard.GetComponent<CardDisplay>().card.category == Card.Category.Field)
        {
                if(FieldPosition.GetComponent<CardPosition>().isOccupied == true)
                {
                    Destroy(FieldPosition.GetComponent<CardPosition>().card);     
                }  
                currentCard.transform.position = FieldPosition.transform.position;
                currentCard.transform.rotation = FieldPosition.transform.rotation;
                currentCard.tag = "Played";
                GameObject field;
                FieldSpellCard currentFieldSpell = (FieldSpellCard)currentCard.GetComponent<CardDisplay>().card;
                field = currentFieldSpell.FieldPrefab;
                Instantiate(field);
                FieldPosition.GetComponent<CardPosition>().isOccupied = true;
                for(int i=0;i<MonsterCardsOnField.Count;i++){
                    MonsterCard monster = (MonsterCard)MonsterCardsOnField[i].GetComponent<CardDisplay>().card;
                    currentFieldSpell.ChangeStats(monster);
                }
    
                currentCard = null;
                

        }

        if(currentCard.tag == "Played")
        {
            //orient model to monster
            //calc attk def lifepoints
            //destroy monster

            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {

                if(hit.collider != null && hit.collider.gameObject.CompareTag("OpponentCard"))
                {
                    MonsterCard card = (MonsterCard)hit.collider.gameObject.GetComponent<CardDisplay>().card;
                    MonsterCard myCard = (MonsterCard)currentCard.GetComponent<CardDisplay>().card;   
                    GameObject oppMonster = hit.collider.gameObject.GetComponent<AkizaMonsters>().monsterObject;   


                    int.TryParse(LifePoints.text, out int lp);
                    int calc;

                    //calculate diff between opponents monster and atting monster
                    calc =  card.Attack - myCard.Attack;
                    //if calc<0 my attk was greater than their attk                    
                    StartCoroutine(AttackAnim(myCard, oppMonster, hit, calc));
                    int templp = lp + calc;
                    StartCoroutine(lifePointCalc(lp, templp));
                }    
            } 
        }
        
    }

    IEnumerator lifePointCalc(int _lp, int _targetlp){
        while(_targetlp < _lp){
            _lp--;
            LifePoints.text = _lp.ToString();
            yield return new WaitForSeconds(0.0001f);
        }
    }


    IEnumerator AttackAnim( MonsterCard _myCard, GameObject _oppMonster, RaycastHit _hit, int _calc){

            DuelController.GetComponent<DuelController>().Attack(_myCard.CardName);
            yield return new WaitForSeconds(7);
            if(_calc <= 0 && isAttacking == false){
               ParticleSystem p = Instantiate(particle, _oppMonster.transform.position, Quaternion.identity);
               p.Play();
                         _oppMonster.SetActive(false);
                         _hit.collider.gameObject.SetActive(false);
                yield return new WaitForSeconds(2);
                p.Stop();
            }
            yield return null;
    }




// XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX



    //playerHand Object is parent of all card objects in hand
    //cardInHand is the list of the card transforms so they can be edited in this script
    //ccardPositionsInHand is the list of the possible positions a card can be in the hand.
    void Hand(){
        for(int i = 0; i<PlayerHand.childCount;i++){
            CardsInHand.Add(PlayerHand.GetChild(i));
        }
        for(int i = 0; i<CardsInHand.Count;i++){
            CardsPositionsInHand[i].GetComponent<CardPosition>().card = CardsInHand[i].gameObject;
            CardsPositionsInHand[i].GetComponent<CardPosition>().isOccupied = true;
        }
    }


    void FillHand(){

        for(int i=0; i<CardPool.Count;i++)
        {
            GameObject newCard;
            newCard = Instantiate(cardPrefab, CardsPositionsInHand[i].transform.position, CardsPositionsInHand[i].transform.rotation, PlayerHand);
            newCard.GetComponent<CardDisplay>().card = CardPool[i];
            newCard.GetComponent<CardDisplay>().setCardImage();
            newCard.name = "Card" + i;
            // CardPool.Remove(CardPool[i]);   
        }

    }


}