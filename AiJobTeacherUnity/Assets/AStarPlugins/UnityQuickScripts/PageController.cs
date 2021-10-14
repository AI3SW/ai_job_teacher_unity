using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

namespace UnityDecoupledBehavior
{
    public class PageController : MonoBehaviour
    {
        Stack<int> pageNumberStack;
        public List<PageElement> pageList;
        public int current
        {
            get;
            private set;
        }
        bool inTransition;
        public PageElement firstPage;
        public TMPro.TextMeshProUGUI titleText;
        public UnityEvent OnAppQuitPrompt;
        public UnityEvent OnGameQuitPrompt;
        
        [System.Serializable]
        public enum pageName
        {
            HomePage = 0,
            Game_Guess,
            Game_Result,
            Job_Catalogue,
            Job_Info,
            Selfie_Phototaking,
            Selfie_PhotoConfirm,
            AboutUs,
            New_Job

        }

        // Start is called before the first frame update
        void Awake()
        {
            pageNumberStack = new Stack<int>();
            foreach (PageElement page in pageList)
            {
                page.canvaPage = page.GetComponent<CanvasGroup>();
                page.deactivatePage();
            }
            firstPage = pageList[current];
            firstPage.activatePage();
            titleText.text = firstPage.PageTitle;
        }

        // Update is called once per frame
        void Update()
        {
            //For android back button
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                goPrevPage();
            }
            /*
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                transitPage((int)pageName.Game_Instruction);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                transitPage((int)pageName.Game_Game);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                transitPage((int)pageName.Game_Result);
            }*/
        }

        public void transitPage(int nextPageId)
        {
            transitPage(pageList[nextPageId]);
        }
        public void transitPage(PageElement nextPage)
        {
            PageElement currPage = pageList[current];
            if (nextPage == currPage ) return;
            else
            {
                inTransition = true;

                
                currPage.fadePage(0, 0.5f, () =>
                {
                    currPage.gameObject.SetActive(false);
                    inTransition = false;
                    currPage.TransitOut?.Invoke();
                });
                currPage.deactivatePageInteraction();

                nextPage.gameObject.SetActive(true);
                nextPage.fadePage(1, 0.5f, () =>
                {
                    nextPage.activatePageInteraction();
                    inTransition = false;
                    titleText.text = nextPage.PageTitle;
                    nextPage.TransitIn?.Invoke();
                    
                });

                current = pageList.FindIndex((PageElement x) => { return nextPage == x;  });

            }
        }

        public void goNextBufferedPage()
        {
            if(nextPage >= 0)
            {
                goNextPage(nextPage);
                nextPage = -1;
            }
            else
            {
                Debug.LogError("page error");
            }


        }
        public void goNextPage(int nextPageId)
        {
            //Debug.Log("pressed");
            if (inTransition) return;
            pageNumberStack.Push(current);
            transitPage(nextPageId);

        }
        public void goPrevPage()
        {
            int prevNum = 0;
            //Debug.Log("Prev Page");
            //Debug.Log(pageNumberStack.Count);
            if (inTransition) return;
            if (isInGame()) {
                Debug.Log("prompt game quit Page");
                nextPage = (int)pageName.Game_Guess;
                OnGameQuitPrompt?.Invoke();
                return;
            }
            if (pageNumberStack.Count > 0)
            {
                
                prevNum = pageNumberStack.Pop();

                Debug.Log("Prev Page " +prevNum);
                transitPage(prevNum);
            } else
            {
                if(pageList[current] != firstPage)
                {
                    Debug.Log("home Page");
                    transitPage(firstPage);
                } else
                {
                    Debug.Log("prompt quit app Page");
                    OnAppQuitPrompt?.Invoke();
                }
                
            }
        }

        int nextPage;
        public void checkBeforeGoNextPage(int pageNum)
        {
            nextPage = pageNum;
            if (inTransition) return;
            if (isInGame())
            {
                Debug.Log("prompt game quit Page");
                OnGameQuitPrompt?.Invoke();
                return;
            } else
            {
                goNextPage(pageNum);
            }
        }

        /// <summary>
        /// if the page name starts with "Game", it is considered 'InGame'
        /// </summary>
        /// <returns></returns>
        public bool isInGame()
        {
            PageElement currentPage = pageList[current];

            //hardcode
            return currentPage.gameObject.name.StartsWith("Game") && firstPage != currentPage;
        }
    }

}