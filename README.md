<a id="readme-top"></a>

<div align="center">

# AR Training System

### Projection-Based Augmented Reality Training for Energy-Saving Behaviour

A Unity-based three-wall PBAR training system developed as part of a Master's thesis at RWTH Aachen University.

<br />

![Unity](https://img.shields.io/badge/Unity-2020.1.17f1-black?logo=unity&logoColor=white)
![CSharp](https://img.shields.io/badge/C%23-Unity%20Scripts-512BD4?logo=csharp&logoColor=white)
![Research](https://img.shields.io/badge/Project-Master's%20Thesis-blue)
![Status](https://img.shields.io/badge/Status-Completed-brightgreen)

<br />

[Explore the Repository](https://github.com/yefana950923-cmd/AR-Training-System)
·
[Projection System Guide](./How%20to%20Use%20the%20Unity%20Projection%20System.pdf)
·
[Projection Validation](./Validation_of_OffAxisProjection_Accuracy.pdf)

</div>

---

<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About the Project</a>
      <ul>
        <li><a href="#research-context">Research Context</a></li>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li><a href="#system-overview">System Overview</a></li>
    <li><a href="#technical-implementation">Technical Implementation</a></li>
    <li><a href="#unity-scripts">Unity Scripts</a></li>
    <li><a href="#repository-contents">Repository Contents</a></li>
    <li><a href="#getting-started">Getting Started</a></li>
    <li><a href="#research-study">Research Study</a></li>
    <li><a href="#citation">Citation</a></li>
    <li><a href="#acknowledgements">Acknowledgements</a></li>
    <li><a href="#license">License</a></li>
  </ol>
</details>

---

## About the Project

This repository contains the core implementation scripts, documentation, questionnaires, study materials, and analysis files developed for a projection-based augmented reality (PBAR) training system for energy-saving behaviour.

The project investigates how an existing head-mounted virtual reality (VR) training system can be adapted to a three-wall projection-based immersive environment.

The original VR training activity was transferred to a PBAR setup in the **iCare laboratory at RWTH Aachen University**. The adapted training scenario represents a residential environment in winter, in which users interact with:

- a heating system,
- a window,
- blinds,
- indoor comfort and air-quality information, and
- energy-related feedback.

The developed system was subsequently evaluated in a pilot study focusing on perceived usability, presence and immersion, cybersickness, and training experience.

> [!NOTE]
> This repository contains the core scripts and research materials associated with the project.  
> It does **not** contain the complete Unity project or all third-party Unity assets.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

### Research Context

This repository accompanies the Master's thesis:

> **AR-Supported Immersive Training to Promote Energy-Saving Behaviour: Concept, Development, and a Pilot Study**

The work was conducted at the **Institute of Energy Efficiency and Sustainable Building (E3D), RWTH Aachen University**.

The project builds upon an existing VR-based energy-saving training system and investigates its transfer to a three-wall PBAR environment.

The main research questions addressed are:

1. How can an existing VR training system be adapted to develop a PBAR system for use in a three-wall laboratory environment?
2. What levels of usability and perceived immersion does the developed PBAR system achieve based on the selected evaluation measures?
3. What differences can be identified between the developed PBAR system and the original VR training system based on the evaluation results?

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

### Built With

The system was primarily developed using:

- [Unity](https://unity.com/) — Unity 2020.1.17f1
- C#
- OpenGLCore
- TextMeshPro (TMP)
- Custom off-axis projection matrices
- Multi-camera / multi-display rendering

The physical PBAR environment consists of three projection walls arranged around the observer.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

## System Overview

The PBAR training system consists of two main components:

### Display System

The original VR camera setup was replaced by a three-camera projection system corresponding to the:

- left projection wall,
- front projection wall,
- right projection wall.

Each view is calculated according to the observer position and the geometry of the corresponding projection plane.

### Interaction System

The original VR interaction logic was adapted to keyboard-based interaction suitable for the projection environment.

Users can control:

- heating level,
- window state,
- blind state,

and navigate through the different stages of the training experiment.

The experimental procedure consists of three Unity scenes:

```text
Scene0
├── Adaptation phase
├── Heating / window / blinds interaction
└── Thermal-comfort questionnaire

        ↓

Scene1
├── Training Attempt 1
│   └── Temperature, air-quality and energy-cost information
│
├── Training Attempt 2
│   └── Additional energy-saving tips
│
└── Training-data recording

        ↓

Scene2
└── Post-experiment questionnaire
```

During the training phase, participants attempt to maintain suitable indoor conditions while avoiding unnecessary energy consumption.

In the first attempt, the information panel displays the predicted indoor temperature, air quality, and estimated monthly energy cost for the selected room settings.

In the second attempt, the same information remains available and additional energy-saving tips are displayed according to the selected heating, window, and blinds states.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

## Technical Implementation

### Three-Wall Projection Environment

The projection environment consists of three physical projection surfaces:

- front wall,
- left wall,
- right wall.

The side walls are arranged approximately 90° relative to the front wall.

Each projection surface is represented geometrically in Unity and rendered through its own camera.

### Off-Axis Projection

A conventional symmetric camera projection assumes that the observer is positioned directly in front of the centre of the display.

This assumption does not hold for a three-wall immersive projection system.

The PBAR system therefore uses **off-axis projection**, in which the viewing frustum is calculated from:

- the observer's eye position,
- the lower-left corner of the projection plane,
- the lower-right corner of the projection plane,
- the upper-left corner of the projection plane.

For each projection wall, a separate asymmetric viewing frustum is generated.

The corresponding implementation is provided in:

```text
unity scripts/OffAxisProjection.cs
```

For additional details, see:

- [`How to Use the Unity Projection System.pdf`](./How%20to%20Use%20the%20Unity%20Projection%20System.pdf)
- [`Validation_of_OffAxisProjection_Accuracy.pdf`](./Validation_of_OffAxisProjection_Accuracy.pdf)

### Multi-Display Output

The left, front, and right camera views are assigned to separate display outputs corresponding to the three physical projectors.

The display activation logic is implemented in:

```text
unity scripts/MultiDisplay.cs
```

### Participant-Specific Eye Height

During the experiment, the observer remained seated at a predefined position.

The horizontal observer position remained fixed, while the eye height was adapted for individual participants.

The resulting eye position was used as the viewpoint for all three off-axis projection cameras.

### Experimental Data Logging

The Unity implementation also includes functionality for recording:

- participant interactions,
- questionnaire responses,
- final room settings,
- interaction counts,
- training attempts.

These data were subsequently used for analysis of the pilot study.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

## Unity Scripts

The Unity scripts used for the PBAR system are located in:

```text
unity scripts/
```

They can be divided into two main groups:

1. TMP and scene-control scripts
2. system-level scripts

---

### TMP and Scene-Control Scripts

The following scripts are located in:

```text
unity scripts/TMP_Control/
```

These scripts mainly control TextMeshPro-based instructions, user-interface elements, questionnaires, scene-specific interaction, and visual feedback.

| Script | Main Function |
|---|---|
| `NamesUI.cs` | Provides general TMP-based user-interface text control. |
| `Scene0_InstructionText.cs` | Controls instructional text displayed during Scene0. |
| `Scene0_Interaction.cs` | Handles heating, window, and blinds interaction during the pre-training phase. |
| `Scene0_NamesUI.cs` | Updates the heating, window, and blinds status displayed in Scene0. |
| `Scene0_Surveys.cs` | Handles the thermal-comfort questionnaire and stores participant responses. |
| `Scene1_InstructionText.cs` | Controls instructional text displayed during the main training phase. |
| `Scene1_NamesUI.cs` | Updates the current heating, window, and blinds status displayed in Scene1. |
| `Scene1_StatusAndComfort.cs` | Handles the main training interaction and updates the virtual environment and associated information according to the selected room state. |
| `Scene1_TipsOnTV.cs` | Displays state-dependent energy-saving tips on the virtual TV during the second training attempt. |
| `Scene2_InstructionText.cs` | Controls instructional text displayed during the post-experiment phase. |
| `Scene2_surveys.cs` | Handles the post-experiment questionnaire, number-key responses, and automatic progression between questions. |

TextMeshPro was used to present instructions, current states, questionnaire items, and energy-related feedback directly within the projected environment.

---

### System-Level Scripts

The scripts located directly in the `unity scripts/` directory provide the main projection, display, experiment-control, and recording functionality.

| Script | Main Function |
|---|---|
| `MultiDisplay.cs` | Activates and configures the multiple Unity display outputs used for the three-wall projection system. |
| `OffAxisProjection.cs` | Calculates the asymmetric projection matrix for each projection wall according to the observer position and projection-plane geometry. |
| `PositionChecker.cs` | Supports checking relevant spatial positions used during configuration and validation of the projection setup. |
| `Scene1_AttemptStatistics.cs` | Records participant interaction counts and the final heating, window, and blinds settings for the two training attempts. |
| `SceneLoader_NextScene.cs` | Controls transitions between experimental scenes and enables progression only when the required condition has been reached. |

---

### OffAxisProjection.cs

`OffAxisProjection.cs` is the main script responsible for the projection calculation used in the final PBAR display system.

For each projection wall, four transforms are used:

```text
pa  → lower-left corner
pb  → lower-right corner
pc  → upper-left corner
eye → observer position
```

The script first calculates the vectors from the observer position to the three corners of the projection plane.

From these positions, the local coordinate system of the projection surface is determined, including:

```text
right direction
up direction
surface normal
```

The corresponding left, right, bottom, and top boundaries of the asymmetric viewing frustum are then calculated.

A custom off-centre perspective matrix is constructed and assigned to the Unity camera.

This allows the three cameras to share the same observer position while using different projection matrices corresponding to the front, left, and right physical projection walls.

---

### Scene0 Interaction

During the adaptation phase, the main keyboard controls are:

| Key | Function |
|---|---|
| `H` | Change heating level |
| `W` | Open / close window |
| `B` | Open / close blinds |

`Scene0_Interaction.cs` maps these inputs to the corresponding room states and refreshes the environment after each interaction.

The thermal-comfort questionnaire is subsequently controlled by `Scene0_Surveys.cs`.

---

### Scene1 Training and Data Recording

Scene1 contains the two main training attempts.

Participants interact with:

```text
Heating
Window
Blinds
```

The selected combination is connected to the information displayed in the training environment.

During Attempt 1, the information panel presents:

- predicted indoor temperature,
- air quality,
- estimated monthly energy cost.

During Attempt 2, additional state-dependent energy-saving tips are displayed.

At the end of each attempt, the participant confirms the final setting using the `Return` key.

`Scene1_AttemptStatistics.cs` records:

- total number of control-key presses,
- final heating state,
- final window state,
- final blinds state.

The two attempts are stored separately.

---

### Scene2 Questionnaire

The post-experiment questionnaire is implemented directly in Unity.

`Scene2_surveys.cs`:

- determines the available response options for each question,
- accepts the corresponding number-key input,
- stores the selected response,
- automatically advances to the next question.

This allows participants to complete the questionnaire directly within the PBAR environment.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

## Repository Contents

```text
AR-Training-System/
│
├── unity scripts/
│   │
│   ├── TMP_Control/
│   │   ├── NamesUI.cs
│   │   ├── Scene0_InstructionText.cs
│   │   ├── Scene0_Interaction.cs
│   │   ├── Scene0_NamesUI.cs
│   │   ├── Scene0_Surveys.cs
│   │   ├── Scene1_InstructionText.cs
│   │   ├── Scene1_NamesUI.cs
│   │   ├── Scene1_StatusAndComfort.cs
│   │   ├── Scene1_TipsOnTV.cs
│   │   ├── Scene2_InstructionText.cs
│   │   └── Scene2_surveys.cs
│   │
│   ├── MultiDisplay.cs
│   ├── OffAxisProjection.cs
│   ├── PositionChecker.cs
│   ├── Scene1_AttemptStatistics.cs
│   └── SceneLoader_NextScene.cs
│
├── Unity_Scripts_Overview.docx
│
├── Final_Survey.xlsx
├── Survey_Comparison_Analysis.xlsx
├── Survey_UNIVPMtoAACHEN.xlsx
│
├── How to Use the Unity Projection System.pdf
├── Validation_of_OffAxisProjection_Accuracy.pdf
├── Participant Information and Consent Form.pdf
│
└── README.md
```

### File Overview

| File | Description |
|---|---|
| `Unity_Scripts_Overview.docx` | Overview and documentation of the Unity scripts used in the project. |
| `Final_Survey.xlsx` | Questionnaire and pilot-study data. |
| `Survey_Comparison_Analysis.xlsx` | Analysis and comparison of questionnaire results. |
| `Survey_UNIVPMtoAACHEN.xlsx` | Mapping between the questionnaire used in the original VR study and the questionnaire used in the PBAR study. |
| `How to Use the Unity Projection System.pdf` | Documentation for configuring and using the Unity projection system. |
| `Validation_of_OffAxisProjection_Accuracy.pdf` | Geometric validation of the implemented off-axis projection method. |
| `Participant Information and Consent Form.pdf` | Participant information and consent material used for the pilot study. |

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

## Getting Started

Clone the repository:

```bash
git clone https://github.com/yefana950923-cmd/AR-Training-System.git
cd AR-Training-System
```

The Unity scripts are available in:

```text
unity scripts/
```

For the projection implementation, the main scripts are:

```text
unity scripts/OffAxisProjection.cs
unity scripts/MultiDisplay.cs
```

For the scene-specific interaction and TMP implementation, see:

```text
unity scripts/TMP_Control/
```

Additional information about the projection setup is available in:

```text
How to Use the Unity Projection System.pdf
```

> [!IMPORTANT]
> This repository is intended primarily as research documentation and supplementary material.
>
> The complete Unity project, virtual apartment environment, and all associated third-party assets are not included. Therefore, cloning this repository alone will not produce a directly executable version of the complete PBAR training application.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

## Research Study

A pilot study with **15 participants** was conducted using the developed PBAR training system.

The evaluation included:

- System Usability Scale (SUS),
- presence and immersion measures,
- cybersickness-related questions,
- training-experience measures,
- visual-characteristic measures,
- energy-related knowledge questions.

The PBAR results were descriptively compared with corresponding data from the original VR study.

The comparison was intended to provide context for evaluating the developed PBAR system and should not be interpreted as a controlled experimental comparison between VR and PBAR display technologies.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

## Citation

If you use the implementation, documentation, or research materials contained in this repository, please cite the repository.

### BibTeX

```bibtex
@misc{ye2026artraining,
  author       = {Ye, Fan},
  title        = {AR Training System: Projection-Based Augmented Reality Training for Energy-Saving Behaviour},
  year         = {2026},
  howpublished = {\url{https://github.com/yefana950923-cmd/AR-Training-System}},
  note         = {GitHub repository}
}
```

### Repository

```text
Fan Ye
AR Training System
https://github.com/yefana950923-cmd/AR-Training-System
```

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

## Acknowledgements

I would like to express my sincere gratitude to **Qirui Huang** for his dedicated supervision, careful guidance, and valuable feedback throughout the development of this project and the preparation of my Master's thesis. His support and rigorous approach were highly valuable to this work.

This project was developed as part of a Master's thesis at **RWTH Aachen University** and carried out at the **Institute of Energy Efficiency and Sustainable Building (E3D)**.

The work builds upon an existing VR-based energy-saving training system developed by researchers at **Marche Polytechnic University (UNIVPM)**.

I would also like to thank all supervisors and researchers who contributed to and supported the development, adaptation, and evaluation of the PBAR training system.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

## License

No explicit open-source license is currently assigned to this repository.

Unless otherwise stated, copyright remains with the respective authors and rights holders.

The public availability of this repository should not be interpreted as granting permission to redistribute, modify, or commercially reuse its contents beyond what is permitted by applicable copyright law.

For academic use, appropriate attribution and citation are requested.

<p align="right">(<a href="#readme-top">back to top</a>)</p>
