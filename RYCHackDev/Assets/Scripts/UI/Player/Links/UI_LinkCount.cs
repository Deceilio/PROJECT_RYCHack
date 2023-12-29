using System.Collections;
using TMPro;
using UnityEngine;

public class UI_LinkCount : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] digitTexts; // Array of Text elements for each digit (6 digits)
    [SerializeField] private float rollDuration = 3.0f; // Duration of rolling animation
    [SerializeField] private float displayDuration = 0.5f; // Duration to display the final number

    private int currentNumber = 0;
    private int targetNumber = 0;
    private int increaseAmount = 10; // Default increase amount

    private int[] powersOf10; // Pre-calculated powers of 10

    private int maxDigits; // Max number of digits in a number
    private int digitTextsLength;

    private const int MaxDigitValue = 9;
    private const int MinSignificantDigitValue = 1;

    private void Start()
    {
        InitializePowersOf10(10); // Set an appropriate value for maxDigits
        digitTextsLength = digitTexts.Length;
        UpdateNumberDisplay(currentNumber); // Initialize the display
    }

    private void InitializePowersOf10(int maxDigits)
    {
        powersOf10 = new int[maxDigits];
        for (int i = 0; i < maxDigits; i++)
        {
            powersOf10[i] = (int)Mathf.Pow(10, i);
        }
    }

    public void ChangeIncreaseAmount(int newAmount)
    {
        increaseAmount = newAmount;
        targetNumber = currentNumber + increaseAmount;
        StartCoroutine(RollAndAnimate());
    }

    private IEnumerator RollAndAnimate()
    {
        StartCoroutine(RollNumbers(currentNumber, targetNumber));

        // Wait for the rolling animation to complete
        yield return new WaitForSeconds(rollDuration);

        StartCoroutine(AnimateNumberChange());
    }

    private IEnumerator RollNumbers(int fromNumber, int toNumber)
    {
        int digitCount = Mathf.Max(Mathf.FloorToInt(Mathf.Log10(toNumber) + 1), 1); // Calculate the number of digits in the target number
        float startTime = Time.time;
        float endTime = startTime + rollDuration;

        while (Time.time < endTime)
        {
            int rolledNumber = 0;
            for (int i = 0; i < digitCount; i++)
            {
                int minDigitValue = i == 0 ? MinSignificantDigitValue : 0;
                int maxDigitValue = i == digitCount - 1 ? Mathf.Clamp(GetDigit(toNumber, i), MinSignificantDigitValue, MaxDigitValue) : MaxDigitValue;

                int randomDigit = Random.Range(minDigitValue, maxDigitValue + 1);
                rolledNumber += randomDigit * powersOf10[i];
            }

            UpdateNumberDisplay(rolledNumber);
            yield return null;
        }
    }

    private IEnumerator AnimateNumberChange()
    {
        float startTime = Time.time;
        float endTime = startTime + displayDuration;

        while (Time.time < endTime)
        {
            float progress = (Time.time - startTime) / displayDuration;

            for (int i = 0; i < digitTextsLength; i++)
            {
                int currentDigit = GetDigit(currentNumber, i);
                int targetDigit = GetDigit(targetNumber, i);
                int newDigit = Mathf.FloorToInt(Mathf.Lerp(currentDigit, targetDigit, progress));

                SetDigit(ref currentNumber, i, newDigit);
                UpdateDigitDisplay(digitTexts[i], newDigit);
            }

            yield return null;
        }

        currentNumber = targetNumber;
        UpdateNumberDisplay(currentNumber);
    }

    private void UpdateDigitDisplay(TextMeshProUGUI digitText, int digitValue)
    {
        digitText.text = digitValue.ToString();
    }

    private int GetDigit(int number, int digitIndex)
    {
        return (number / powersOf10[digitIndex]) % 10;
    }

    private void SetDigit(ref int number, int digitIndex, int newDigit)
    {
        number += (newDigit - GetDigit(number, digitIndex)) * powersOf10[digitIndex];
    }

    private void UpdateNumberDisplay(int number)
    {
        for (int i = 0; i < digitTextsLength; i++)
        {
            int digitValue = GetDigit(number, i);
            UpdateDigitDisplay(digitTexts[i], digitValue);
        }
    }
}
