using System;
using System.Media;

namespace Tund5;

class HeliTegija
{
    private SoundPlayer bgMusic;
    private SoundPlayer eatSound;
    private SoundPlayer gameOverSound;

    public HeliTegija()
    {
        try
        {
            bgMusic = new SoundPlayer("../../../sounds/bg.wav");
            eatSound = new SoundPlayer("../../../sounds/eat.wav");
            gameOverSound = new SoundPlayer("../../../sounds/gameover.wav");
        }
        catch { }
    }

    public void PlayBackground()
    {
        try
        {
            bgMusic.PlayLooping();
        }
        catch { }
    }

    public void StopBackground()
    {
        try
        {
            bgMusic.Stop();
        }
        catch { }
    }

    public void PlayEat()
    {
        try
        {
            eatSound.Play();
        }
        catch { }
    }

    public void PlayGameOver()
    {
        try
        {
            gameOverSound.Play();
        }
        catch { }
    }
}