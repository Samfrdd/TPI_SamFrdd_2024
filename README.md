# Résolution d’un labyrinthe par la méthode d’un arbre de recherche

## Description
Ce projet, réalisé dans le cadre du TPI (Travail Pratique Individuel), vise à créer une application capable de :

1. Générer aléatoirement des labyrinthes en 3D à l'aide de blocs prédéfinis.
2. Trouver le(s) chemin(s) le(s) plus court(s) entre une entrée et une sortie grâce à un algorithme basé sur un arbre de recherche.
3. Proposer trois modes de simulation : manuel, semi-automatique et automatique.
4. Fournir des fonctionnalités telles que la sauvegarde et le chargement des labyrinthes, ainsi que l'affichage d'informations détaillées sur les performances.

## Fonctionnalités principales
- **Génération aléatoire** : Création de labyrinthes à partir de blocs connectables.
- **Modes de recherche** :
  - **Manuel** : Recherche d'un chemin entre une entrée et une sortie choisies par l'utilisateur.
  - **Semi-automatique** : Identification de toutes les sorties possibles depuis une entrée donnée.
  - **Automatique** : Analyse complète du labyrinthe, incluant le score, les distances parcourues et les temps nécessaires.
- **Sauvegarde et chargement** : Sauvegarde des labyrinthes avec nom personnalisé, classement des meilleurs selon un score.
- **Interface utilisateur intuitive** : Navigation dans les menus, sélection des modes, et visualisation des performances des bots.

## Prérequis

### Matériel
- Poste de travail sous Windows 10 Pro.
- Clé USB (16 Go) et SSD dédié (250 Go).

### Outils et logiciels
- **Visual Studio Code** : v1.82.0
- **Unity3D** : v2022.3.17f1
- **Git** : v2.34.1
- **Google Drive** : pour les sauvegardes cloud.

## Structure des dossiers
- **Assets/** : Contient les éléments Unity (scripts, scènes, préfabs, etc.).
- **Documentation/** : Documentation technique et livrables.
- **Scripts/** : Tous les scripts du projet.
- **Prefabs/** : Préfabriqués pour les blocs, bots, et UI.

## Méthodologie de développement
Le projet a été réalisé selon une **méthode en 6 étapes** :
1. **S'informer** : Comprendre les besoins et les contraintes.
2. **Planifier** : Déterminer les tâches et estimer leur durée.
3. **Décider** : Choisir les solutions techniques.
4. **Réaliser** : Implémenter les fonctionnalités.
5. **Contrôler** : Tester et valider les résultats.
6. **Évaluer** : Identifier les points d'amélioration.

Une méthode de sauvegarde **3-2-1** a été utilisée pour garantir la sécurité des données.

## Tests
Chaque fonctionnalité a été rigoureusement testée via des scénarios définis, couvrant :
- La génération de labyrinthes.
- Le placement des entrées et sorties.
- La performance des algorithmes de recherche.
- La gestion des sauvegardes.

## Améliorations futures
- Optimisation de la génération des labyrinthes pour réduire le temps de calcul.
- Meilleure organisation et factorisation du code.
- Amélioration de l'interface utilisateur et des visuels.

## Auteurs
- **Sam FREDDI**
- **Maître d’apprentissage** : Pascal Henauer
- **Experts** : Thomas Tetart, Brian Nydegger

## Licence
Ce projet est réalisé dans un cadre éducatif. Toute réutilisation ou redistribution doit être approuvée par l'auteur.
