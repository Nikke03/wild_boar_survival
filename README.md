
<!DOCTYPE html>
<html lang="it">

<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>README - Il mio videogioco 2D</title>
  <style>
    body {
      font-family: Arial, sans-serif;
      line-height: 1.6;
      margin: 20px;
      max-width: 800px;
      margin: auto;
    }

    h1,
    h2,
    h3 {
      color: #333;
    }

    p {
      color: #555;
    }

    .container {
      display: flex;
      flex-direction: column;
      align-items: center;
    }

    .image-section {
      margin: 20px 0;
      text-align: center;
    }

    .image-section img {
      max-width: 100%;
      height: auto;
      border: 2px solid #ddd;
      border-radius: 4px;
      box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
    }

    .sprite-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(100px, 1fr));
      gap: 15px;
      margin-top: 20px;
    }

    .sprite-grid img {
      width: 100%;
      border: 2px solid #ddd;
      border-radius: 4px;
      box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
    }

    .video-section {
      margin: 20px 0;
      text-align: center;
    }

    .video-section iframe {
      width: 100%;
      aspect-ratio: 16 / 9;
      border: none;
    }
  </style>
</head>

<body>

  <div class="container">

    <!-- Introduzione -->
    <h1>Il mio videogioco 2D</h1>
    <p>Descrizione generale del progetto (da personalizzare).</p>

    <!-- Immagine principale -->
    <div class="image-section">
      <img src="https://via.placeholder.com/800x400" alt="Immagine principale del gioco">
      <p>Immagine descrittiva del gioco.</p>
    </div>

    <!-- Video gameplay -->
    <div class="video-section">
      <h2>Gameplay</h2>
      <iframe src="https://www.youtube.com/embed/dQw4w9WgXcQ" title="YouTube video player" frameborder="0"
        allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
        allowfullscreen></iframe>
      <p>Guarda il gameplay in azione!</p>
    </div>

    <!-- Sezione sprite -->
    <div class="image-section">
      <h2>Sprites di animazione</h2>
      <div class="sprite-grid">
        <img src="https://via.placeholder.com/100x100" alt="Sprite 1">
        <img src="https://via.placeholder.com/100x100" alt="Sprite 2">
        <img src="https://via.placeholder.com/100x100" alt="Sprite 3">
        <img src="https://via.placeholder.com/100x100" alt="Sprite 4">
        <img src="https://via.placeholder.com/100x100" alt="Sprite 5">
        <!-- Aggiungi più immagini qui -->
      </div>
      <p>Gli sprite utilizzati per le animazioni nel gioco.</p>
    </div>

    <!-- Altre immagini lunghe -->
    <div class="image-section">
      <h2>Immagini del gioco</h2>
      <img src="https://via.placeholder.com/800x600" alt="Immagine lunga 1">
      <img src="https://via.placeholder.com/800x600" alt="Immagine lunga 2">
      <img src="https://via.placeholder.com/800x600" alt="Immagine lunga 3">
      <p>Queste immagini mostrano alcune scene chiave del gioco.</p>
    </div>

    <!-- Call to action -->
    <h2>Come giocare</h2>
    <p>Istruzioni su come eseguire il gioco (da personalizzare).</p>

    <h2>Contatti</h2>
    <p>Se hai domande o feedback, contattami all'indirizzo email@example.com.</p>

  </div>

</body>

</html>
