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

The main research questions addressed:

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

The training consists of two attempts.

#### Attempt 1 — Initial Interaction

Participants interact with the residential environment with limited feedback while attempting to maintain suitable indoor conditions and avoid unnecessary energy use.

#### Attempt 2 — Enhanced Feedback

Additional visual information is provided, including:

- predicted indoor temperature,
- indoor air-quality information,
- estimated monthly energy costs,
- real-time energy-saving feedback.

This allows the training activity to provide more explicit information about the consequences of user decisions.

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

The corresponding implementation is available in the Unity scripts included in this repository.

For additional details, see:

- [`How to Use the Unity Projection System.pdf`](./How%20to%20Use%20the%20Unity%20Projection%20System.pdf)
- [`Validation_of_OffAxisProjection_Accuracy.pdf`](./Validation_of_OffAxisProjection_Accuracy.pdf)

### Participant-Specific Eye Height

During the experiment, the observer remained seated at a predefined position.

The horizontal observer position remained fixed, while the eye height was adapted for individual participants.

The resulting eye position was used as the viewpoint for all three off-axis projection cameras.

### Experimental Data Logging

The Unity implementation also includes functionality for recording:

- participant interactions,
- questionnaire responses,
- experiment states,
- training attempts.

These data were subsequently used for analysis of the pilot study.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

## Repository Contents

```text
AR-Training-System/
│
├── Questionnaire/
│   └── Questionnaire and study-related materials
│
├── unity scripts/
│   └── Core Unity C# scripts used in the implementation
│
├── Final_Survey.xlsx
│   └── Final questionnaire / survey data
│
├── Survey_Comparison_Analysis.xlsx
│   └── Analysis and comparison of questionnaire results
│
├── Survey_UNIVPMtoAACHEN.xlsx
│   └── Mapping between the original VR questionnaire
│       and the adapted PBAR questionnaire
│
├── How to Use the Unity Projection System.pdf
│   └── Documentation for configuring the Unity
│       projection system
│
├── Validation_of_OffAxisProjection_Accuracy.pdf
│   └── Geometric validation of the off-axis
│       projection implementation
│
├── Participant Information and Consent Form.pdf
│   └── Participant information and consent material
│
└── README.md
