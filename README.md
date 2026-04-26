GD3_Cavallaro_TD1_NM
Moteur : Unity 6000.2 — Pipeline : Universal Render Pipeline (URP) — Plateforme cible : PC

Jeu de plateau 3D à thème égyptien développé en solo dans le cadre d'un TD de Game Design 3. Le joueur progresse sur un plateau circulaire en lançant un dé, tombe sur des cases à effets variés, collecte des clés et complète une quête pour débloquer la victoire. Un mini-jeu d'infiltration en vue troisième personne s'intercale en cours de partie.

Scènes
Scène	Rôle

MainMenu	Menu principal avec lancement de partie et quitter

LoopHeroScene	Scène principale — plateau de jeu, système de dé et de quête

MiniGameSceneName	Mini-jeu d'infiltration 3D en désert

NatureMorteScene	Scène secondaire (décor nature morte)

Gameplay — Plateau principal (LoopHeroScene)

- Dé et déplacement
- Le joueur lance un dé (valeur 1 ou 2) via un bouton UI.
- Le pion se déplace en interpolation fluide (Vector3.MoveTowards) vers la case cible, avec animation de marche pilotée par le paramètre Speed de l'Animator.
- Le plateau est circulaire : le pion revient à la case 0 une fois le dernier index dépassé.
- Le bouton dé est masqué pendant le déplacement et pendant les dialogues, puis réaffiché automatiquement.

Types de cases
Type	Comportement

Case neutre Cell	Aucun effet — classe de base extensible

Case piège TrapCell	Inflige des dégâts configurables, effet particules, déclenchement unique ou répété

Case soin HealCell	Restaure des PV configurables sans dépasser le maximum, usage unique ou multiple

Case dialogue DialogueCell	Lance un dialogue contextuel selon l'état de visite et les clés obtenues ; active la quête ; déclenche la victoire si toutes les conditions sont remplies

Case trésor TreasureCell	Coffre lié à la quête ; choix : ouverture délicate (sans dégâts) ou forçage rapide (dégâts) ; octroie la première clé

Case mini-jeu MiniGameCell	Sauvegarde la santé et charge le mini-jeu si la deuxième clé n'a pas été obtenue

Case mini-jeu ParkourCell  Sauvegarde la santé et charge le mini-jeu si la troisième clé n'a pas été obtenue

Système de quête
- QuestCondition est un ScriptableObject avec trois états : inactif, actif, complété.
- La quête s'active au premier passage sur la DialogueCell.
- Elle se complète lors de l'interaction avec le coffre (TreasureCell).
- La victoire finale requiert : quête complétée et 2 clés obtenues.

Gameplay — Mini-jeu (MiniGameSceneName)
Scène d'infiltration en désert avec ennemis de type momie.


Contrôleur joueur (MiniGamePlayerController)
- Déplacement 3D relatif à la caméra via le New Input System.
- Gravité appliquée par CharacterController.
- Rotation fluide du visuel vers la direction de déplacement (Quaternion.Slerp).
- Son de pas alterné entre deux clips avec cooldown configurable.
- Trigger d'animation Victory à la collecte du coffre.

IA ennemie (EnemyAI — NavMesh)
- Patrouille entre waypoints en boucle.
- Détection : passe en poursuite si le joueur entre dans le rayon de détection.
- Poursuite : déplacement vers le joueur à vitesse accrue ; son d'alerte au premier contact.
- Épuisement : après maxChaseDuration secondes, l'ennemi récupère pendant exhaustionDuration secondes puis reprend sa patrouille.
- Attaque : animation corps à corps dont la durée est calée sur le clip réel.
- Audio 3D spatial : grognements périodiques et cri d'alerte, rolloff linéaire par instance.
- Gizmos éditeur : sphère rouge (détection), sphère jaune (attaque).

Gestion du mini-jeu (MiniGamesManager)
- Coffre touché → clé incrémentée, ennemis désactivés, animation victoire, retour à LoopHeroScene.
- Ennemi touche le joueur → −25 sur savedHealth. Si mort : Game Over, retour sans clé.
- La santé persiste entre scènes via PlayerData (ScriptableObject).

Gestion du mini-jeu (Parkour)
- Le joueur collecte 20 pièces -> il gagne la clef 
- Le joueur tombe, il perds 25 PV

Systèmes transversaux
Santé (HealthSystem)
- Événements C# : OnHealthChanged(int current, int max) et OnDeath.
- Méthodes : TakeDamage, Heal, ResetHealth, SetCurrentHealth.
- Son de dégâts joué automatiquement via AudioManager.

Dialogue (DialogueManager / DialogueData)
- DialogueData ScriptableObject : liste de DialogueLine (nom + texte) et DialogueChoice avec ChoiceOutcome.
- Séquence ligne par ligne, choix dynamiques, enchaînement de dialogues via ChoiceOutcomeType.StartDialogue.
- Caméra et bouton dé désactivés automatiquement pendant un dialogue (via événements).

Persistance entre scènes (PlayerData)
- ScriptableObject partagé : position sur le plateau, santé sauvegardée, nombre de clés, drapeau de retour du mini-jeu.
- GameInitializer réinitialise tout en nouvelle partie ou après Game Over, et restaure la santé sauvegardée au retour du mini-jeu.

Audio (AudioManager)
- Singleton DontDestroyOnLoad, deux AudioSource séparées (musique boucle / SFX).
- Musiques contextuelles : menu, plateau, mini-jeu, victoire.
- SFX : dé, dégâts, game over, pas (alternance A/B), coffre, fanfare victoire.
- Volumes musique et SFX ajustables à l'exécution.

Caméra (CameraController)
- Orbite autour du pion avec clic droit maintenu.
- Zoom molette avec plage min/max configurable.
- Suivi fluide en Lerp avec offset vertical.
- Clamp des angles verticaux.
- Contrôles suspendus pendant les dialogues.

Game Over (GameOverManager)
- S'abonne à OnDeath du HealthSystem.
- Affiche le panneau, gèle le temps (Time.timeScale = 0), propose relancer ou menu.

Victoire (VictoryManager)
- Déclenché par DialogueCell quand les 3 clés sont obtenues et la quête complétée.
- Lance la musique de victoire, masque le bouton dé, affiche le panneau après délai.

Interface utilisateur (HUD)
Élément		Script		Description

Barre de vie	HealthUI	Texte TMPro HP: X / 100, mis à jour par événement

Compteur de clés	KeyUI	Texte TMPro X/2, mis à jour chaque frame

Bouton dé	DiceButtonManager	Masqué pendant déplacement et dialogues

Panneau dialogue	DialogueUI	Lignes et boutons de choix dynamiques

Panneau Game Over	GameOverManager	Rejouer / Menu / Quitter

Panneau victoire	VictoryManager	Prochain niveau / Menu

Architecture technique
Patterns utilisés
- Singleton : AudioManager, DialogueManager, DiceButtonManager, GameOverManager, VictoryManager, MiniGamesManager
- ScriptableObject : PlayerData (état persistant inter-scènes), QuestCondition (état de quête), DialogueData (contenu narratif)
- Événements C# : couplage faible entre HealthSystem → HealthUI, DialogueManager → CameraController / DiceButtonManager
- Polymorphisme : hiérarchie de cases via Cell / ICellActivable et surcharge de Activate(Pawn)
- Coroutine : déplacement fluide du pion, séquence de victoire

Packages
- com.unity.inputsystem 1.14.2 — New Input System
- com.unity.ai.navigation 2.0.9 — NavMesh pour l'IA
- com.unity.render-pipelines.universal 17.2.0 — URP (profils PC et Mobile)
- com.unity.ugui 2.0.0 — UI Canvas + TextMesh Pro

Structure des assets
Assets/
├── Audio/                  # Musiques et SFX
│   └── Music/              # AudioManager.cs + clips
├── Characters/
│   ├── Hero/               # Modèle, animations (Idle, Running, Victory), matériaux
│   └── Mommy/              # Modèle mummy, animations, prefabs EnemyAI
├── LoopHero/               # Prefabs de cases et matériaux du plateau
├── Materials/              # Matériaux sol sable, murs briques (PBR complet)
├── NatureMorte/            # Décors statiques (hache, maison, table)
├── Scenes/                 # 4 scènes + NavMesh baked
├── Script/
│   ├── AI/                 # EnemyAI.cs
│   ├── Dialogues/          # DialogueData, DialogueManager, DialogueUI + assets SO
│   ├── Dice/               # Dice.cs, DiceButtonManager.cs
│   ├── Managers/           # GameInitializer, GameOver, Victory, MiniGames, MainMenu
│   ├── Player/             # Pawn, MiniGamePlayerController, CameraController, HealthSystem, PlayerData
│   ├── QuestCondition/     # QuestCondition SO, ChestInteraction
│   ├── ScriptCell/         # Board, Cell, TrapCell, HealCell, DialogueCell, TreasureCell, MiniGameCell
│   └── UI/                 # HealthUI, KeyUI, PanelFadeIn
├── Settings/               # Profils URP PC / Mobile
└── textures/               # Environnement (pyramides, rochers, chameau, coffre)