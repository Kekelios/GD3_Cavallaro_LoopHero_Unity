Loop Hero – Projet Unity

Jeu de plateau narratif avec mécaniques RPG légères, système de dés et dialogues interactifs.
Projet développé sous Unity 6000.2, utilisant URP et C#.


Table des matières

Description

Genre

Gameplay

Fonctionnalités

Architecture et systèmes

Structure du projet

Technologies utilisées

Installation

Comment jouer

Patterns de conception



Description

Loop Hero est un jeu de plateau en boucle dans lequel le joueur progresse case par case en lançant un dé.
Chaque déplacement peut déclencher des événements variés : soins, pièges, trésors ou dialogues à choix multiples.
Le jeu repose sur une logique simple mais extensible, combinant gestion de la santé, narration interactive et conditions de victoire scénarisées.

Le projet met l’accent sur une architecture claire, orientée systèmes, avec une forte utilisation des ScriptableObjects et des événements pour découpler gameplay et interface.



Genre

Jeu de plateau (Board Game)

RPG léger

Aventure narrative



Gameplay

Lancement d’un dé (valeurs possibles : 1 à 3) pour déterminer le déplacement

Déplacement automatique du pion sur un plateau circulaire

Plateau en boucle : retour à la première case après la dernière

Activation automatique de la case atteinte

Gestion des points de vie avec possibilité de Game Over

Dialogues interactifs avec choix influençant le déroulement de la partie

Condition de victoire basée sur des objectifs définis



Fonctionnalités
Système de jeu

Lancer de dé avec génération aléatoire contrôlée

Déplacement fluide du pion

Gestion d’un plateau en boucle

Activation contextuelle des cases



Système de dialogues

Dialogues multi-lignes avec nom du personnage

Choix interactifs impactant le gameplay

Synchronisation UI / gameplay via événements

Dialogues définis via ScriptableObjects

Désactivation automatique des contrôles pendant les dialogues



Système de santé

Points de vie avec maximum configurable

Gestion des soins et des dégâts

Interface affichant les HP actuels et maximum

Détection de la mort et déclenchement du Game Over

Effets sonores associés aux actions



Système audio

Gestion centralisée de la musique et des effets sonores

AudioManager singleton persistant entre les scènes

Deux AudioSource distincts (musique / SFX)



Caméra orbite

Suivi automatique du pion avec interpolation

Rotation contrôlée à la souris (clic droit)

Zoom via molette avec limites configurables

Limitation des angles verticaux

Désactivation automatique pendant les dialogues

Compatibilité avec le nouveau Input System



Victoire et Game Over

Conditions de victoire définies via ScriptableObjects

Écran de Game Over avec possibilité de recommencer

Écran de victoire avec animation

Sauvegarde des données du joueur



Types de cases

Case normale : aucun effet

Case de soin : restaure des points de vie

Case piège : inflige des dégâts

Case trésor : déclenche un événement spécial

Case dialogue : lance une conversation interactive