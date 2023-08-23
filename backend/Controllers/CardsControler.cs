using backend.DTOs;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsControler : ControllerBase
    {
        private ICardRepository _cardRepository;
        private IClientRepository _clientRepository;

        public CardsControler(ICardRepository CardRepository, IClientRepository ClientRepository)
        {
            _cardRepository = CardRepository;
            _clientRepository = ClientRepository;
        }
        [HttpGet("clients/current/cards")]
        public IActionResult Get()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return Forbid();
                }
                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return Forbid();
                }
                var cardsDTO = new List<CardDTO>();
                foreach (Card card in client.Cards)
                {
                    var newCardDTO = new CardDTO()
                    {
                        Id = card.Id,
                        Type = card.Type,
                        Number = card.Number,
                        DeadLine=card.Deadline
                        //agregar info aca
                    };
                    cardsDTO.Add(newCardDTO);
                }
                return Ok(cardsDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        public IActionResult Post([FromBody] Card card)
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return Forbid();
                }

                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return Forbid();
                }

                Card auxCard = _cardRepository.FindById(card.Id);
                if (auxCard == null)
                {
                    Card newCard = new Card()
                    {
                        ClientId = client.Id,
                        Type = card.Type,
                        Number = card.Number,
                        Deadline = card.Deadline,
                    };
                    _cardRepository.Save(newCard);
                    return Created("", newCard);
                }
                else
                {
                    auxCard.Type = card.Type;
                    auxCard.Number = card.Number;
                    auxCard.Deadline = card.Deadline;

                    _cardRepository.Save(auxCard);
                    return Ok(auxCard);
                }      

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

