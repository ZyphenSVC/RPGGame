using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PipeController : MonoBehaviour {
    private const float PIPE_WIDTH = 8f;
    private const float PIPE_HEAD_HEIGHT = 8f;
    private const float ORTHO_SIZE = 50f;
    private const float LEFT_EDGE = -100f;
    private const float RIGHT_EDGE = 100f;
    public const float MOVE_SPEED = 40f;
    private int passCount;
    private List<Pipe> pipeList;
    private float spawnTimer;
    private float spawnTimerMax;
    private float size;
    private int pipesSpawned;
    private State state;
    
    private enum State {
        Waiting,
        Playing,
        Dead,
        Win,
    }
    
    private static PipeController instance;

    public static PipeController GetInstance() {
        return instance;
    }

    private void Awake() {
        instance = this;
        pipeList = new List<Pipe>();
        spawnTimerMax = 1f;
        size = 30f;
        state = State.Waiting;
    }

    private void Start() {
        FlapController.GetInstance().died += onDied;
        FlapController.GetInstance().startPlaying += onStartPlaying;
        FlapController.GetInstance().winning += onWin;
    }

    void onStartPlaying(object sender, System.EventArgs e) {
        state = State.Playing;
    }
    
    void onDied(object sender, System.EventArgs e) {
        state = State.Dead;
    }

    void onWin(object sender, System.EventArgs e) {
        state = State.Win;
    }

    private void Update() {
        if (state == State.Playing) {
            pipeMove();
            pipeSpawn();
            if (passCount >= 20) {
                state = State.Win;
            }
        }
    }

    void pipeSpawn() {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer < 0) {
            spawnTimer += spawnTimerMax;
            float heightLim = 10f;
            float minHeight = size * .5f + heightLim;
            float totalHeight = ORTHO_SIZE * 2f;
            float maxHeight = totalHeight - size * .5f - heightLim;
            
            float height = Random.Range(minHeight, maxHeight);
            gapPipe(height, size, RIGHT_EDGE);
        }
    }

    void pipeMove() {
        for (int i = 0; i < pipeList.Count; i++) {
            Pipe pipe = pipeList[i];
            bool passPlayer = pipe.GetXPos() > 0f;
            pipe.Move();
            if (passPlayer && pipe.GetXPos() <= 0f && pipe.isBottom())
                passCount++;
            if (pipe.GetXPos() < LEFT_EDGE) {
                pipe.destroySelf();
                pipeList.Remove(pipe);
                i--;
            }
        }
        
    }

    void gapPipe(float y, float size, float x) {
        createPipe(y - size * .5f, x, true);
        createPipe(ORTHO_SIZE * 2f - y - size * .5f, x, false);
        pipesSpawned++;
    }
    
    void createPipe(float height, float xpos, bool bot) {
        Transform pipeHead = Instantiate(Assets.GetInstance().transPipeHead);
        float phYPos;
        if (bot)
            phYPos = -ORTHO_SIZE + height - PIPE_HEAD_HEIGHT * .5f;
        else
            phYPos = ORTHO_SIZE - height + PIPE_HEAD_HEIGHT * .5f;
        pipeHead.position = new Vector3(xpos, phYPos);
        
        Transform pipeBody = Instantiate(Assets.GetInstance().transPipeBody);
        float pbYPos;
        if (bot)
            pbYPos = -ORTHO_SIZE;
        else {
            pbYPos = ORTHO_SIZE;
            pipeBody.localScale = new Vector3(1, -1, 1);
        }
        pipeBody.position = new Vector3(xpos, pbYPos);
        
        SpriteRenderer pbSR = pipeBody.GetComponent<SpriteRenderer>();
        pbSR.size = new Vector2(PIPE_WIDTH, height);

        BoxCollider2D pbBC = pipeBody.GetComponent<BoxCollider2D>();
        pbBC.size = new Vector2(PIPE_WIDTH, height);
        pbBC.offset = new Vector2(0f, height * .5f);
        
        Pipe pipe = new Pipe(pipeHead, pipeBody, bot);
        pipeList.Add(pipe);
    }

    public int GetPipesSpawned() {
        return pipesSpawned;
    }

    public int GetPassCount() {
        return passCount;
    }

    class Pipe {
        private Transform pipeHeadTransform;
        private Transform pipeBodyTransform;
        private bool bot;

        public Pipe(Transform pipeHeadTransform, Transform pipeBodyTransform, bool bot) {
            this.pipeHeadTransform = pipeHeadTransform;
            this.pipeBodyTransform = pipeBodyTransform;
            this.bot = bot;
        }

        public void Move() {
            pipeHeadTransform.position += new Vector3(-1, 0, 0) * MOVE_SPEED * Time.deltaTime;
            pipeBodyTransform.position += new Vector3(-1, 0, 0) * MOVE_SPEED * Time.deltaTime;
        }

        public float GetXPos() {
            return pipeHeadTransform.position.x;
        }

        public void destroySelf() {
            Destroy(pipeHeadTransform.gameObject);
            Destroy(pipeBodyTransform.gameObject);
        }

        public bool isBottom() {
            return bot;
        }
        
    }
    
}
